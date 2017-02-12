namespace QuickGraph.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Xml;

    /// <summary>A GraphML ( http://graphml.graphdrawing.org/ ) format deserializer.</summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <typeparam name="TGraph">type of the graph</typeparam>
    /// <remarks>
    ///     <para>
    ///         Custom vertex, edge and graph attributes can be specified by using the
    ///         <see cref="System.Xml.Serialization.XmlAttributeAttribute" />
    ///         attribute on properties (field not suppored).
    ///     </para>
    ///     <para>
    ///         The serializer uses LCG (lightweight code generation) to generate the methods that writes the attributes to
    ///         avoid paying the price of Reflection on each vertex/edge. Since nothing is for free, the first time you will
    ///         use the serializer *on a particular pair of types*, it will have to bake that method.
    ///     </para>
    ///     <para>Hyperedge, nodes, nested graphs not supported.</para>
    /// </remarks>
    public sealed class GraphMlDeserializer<TVertex, TEdge, TGraph>
        : SerializerBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>
    {
        public void Deserialize(
            XmlReader reader,
            TGraph visitedGraph,
            IdentifiableVertexFactory<TVertex> vertexFactory,
            IdentifiableEdgeFactory<TVertex, TEdge> edgeFactory)
        {
            Contract.Requires(reader != null);
            Contract.Requires(visitedGraph != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);

            var worker = new ReaderWorker(
                this,
                reader,
                visitedGraph,
                vertexFactory,
                edgeFactory);
            worker.Deserialize();
        }

        private class ReaderWorker
        {
            private readonly IdentifiableEdgeFactory<TVertex, TEdge> _edgeFactory;

            private readonly IdentifiableVertexFactory<TVertex> _vertexFactory;

            private string _graphMlNamespace = "";

            public GraphMlDeserializer<TVertex, TEdge, TGraph> Serializer { get; }

            public XmlReader Reader { get; }

            public TGraph VisitedGraph { get; }

            public ReaderWorker(
                GraphMlDeserializer<TVertex, TEdge, TGraph> serializer,
                XmlReader reader,
                TGraph visitedGraph,
                IdentifiableVertexFactory<TVertex> vertexFactory,
                IdentifiableEdgeFactory<TVertex, TEdge> edgeFactory
            )
            {
                Contract.Requires(serializer != null);
                Contract.Requires(reader != null);
                Contract.Requires(visitedGraph != null);
                Contract.Requires(vertexFactory != null);
                Contract.Requires(edgeFactory != null);

                Serializer = serializer;
                Reader = reader;
                VisitedGraph = visitedGraph;
                _vertexFactory = vertexFactory;
                _edgeFactory = edgeFactory;
            }

            public void Deserialize()
            {
                ReadHeader();
                ReadGraphHeader();
                ReadElements();
            }

            private static string ReadAttributeValue(XmlReader reader, string attributeName)
            {
                Contract.Requires(reader != null);
                Contract.Requires(attributeName != null);
                reader.MoveToAttribute(attributeName);
                if (!reader.ReadAttributeValue())
                {
                    throw new ArgumentException("missing " + attributeName + " attribute");
                }
                return reader.Value;
            }

            private void ReadEdge(Dictionary<string, TVertex> vertices)
            {
                Contract.Requires(vertices != null);
                Contract.Assert(
                    Reader.NodeType == XmlNodeType.Element &&
                    Reader.Name == "edge" &&
                    Reader.NamespaceURI == _graphMlNamespace);

                // get subtree
                using (var subReader = Reader.ReadSubtree())
                {
                    // read id
                    var id = ReadAttributeValue(Reader, "id");
                    var sourceid = ReadAttributeValue(Reader, "source");
                    TVertex source;
                    if (!vertices.TryGetValue(sourceid, out source))
                    {
                        throw new ArgumentException("Could not find vertex " + sourceid);
                    }
                    var targetid = ReadAttributeValue(Reader, "target");
                    TVertex target;
                    if (!vertices.TryGetValue(targetid, out target))
                    {
                        throw new ArgumentException("Could not find vertex " + targetid);
                    }

                    var edge = _edgeFactory(source, target, id);
                    ReadDelegateCompiler.SetEdgeDefault(edge);

                    // read data
                    while (subReader.Read())
                        if (Reader.NodeType == XmlNodeType.Element &&
                            Reader.Name == "data" &&
                            Reader.NamespaceURI == _graphMlNamespace)
                        {
                            ReadDelegateCompiler.EdgeAttributesReader(subReader, _graphMlNamespace, edge);
                        }

                    VisitedGraph.AddEdge(edge);
                }
            }

            private void ReadElements()
            {
                Contract.Requires(
                    Reader.Name == "graph" &&
                    Reader.NamespaceURI == _graphMlNamespace,
                    "incorrect reader position");

                ReadDelegateCompiler.SetGraphDefault(VisitedGraph);

                var vertices = new Dictionary<string, TVertex>(StringComparer.Ordinal);

                // read vertices or edges
                var reader = Reader;
                while (reader.Read())
                    if (reader.NodeType == XmlNodeType.Element &&
                        reader.NamespaceURI == _graphMlNamespace)
                    {
                        switch (reader.Name)
                        {
                            case "node":
                                ReadVertex(vertices);
                                break;
                            case "edge":
                                ReadEdge(vertices);
                                break;
                            case "data":
                                ReadDelegateCompiler.GraphAttributesReader(Reader, _graphMlNamespace, VisitedGraph);
                                break;
                            default:
                                throw new InvalidOperationException(string.Format("invalid reader position {0}:{1}", Reader.NamespaceURI, Reader.Name));
                        }
                    }
            }

            private void ReadGraphHeader()
            {
                if (!Reader.ReadToDescendant("graph", _graphMlNamespace))
                {
                    throw new ArgumentException("graph node not found");
                }
            }

            private void ReadHeader()
            {
                // read flow until we hit the graphml node
                while (Reader.Read())
                    if (Reader.NodeType == XmlNodeType.Element &&
                        Reader.Name == "graphml")
                    {
                        _graphMlNamespace = Reader.NamespaceURI;
                        return;
                    }

                throw new ArgumentException("graphml node not found");
            }

            private void ReadVertex(Dictionary<string, TVertex> vertices)
            {
                Contract.Requires(vertices != null);
                Contract.Assert(
                    Reader.NodeType == XmlNodeType.Element &&
                    Reader.Name == "node" &&
                    Reader.NamespaceURI == _graphMlNamespace);

                // get subtree
                using (var subReader = Reader.ReadSubtree())
                {
                    // read id
                    var id = ReadAttributeValue(Reader, "id");

                    // create new vertex
                    var vertex = _vertexFactory(id);

                    // apply defaults
                    ReadDelegateCompiler.SetVertexDefault(vertex);

                    // read data
                    while (subReader.Read())
                        if (Reader.NodeType == XmlNodeType.Element &&
                            Reader.Name == "data" &&
                            Reader.NamespaceURI == _graphMlNamespace)
                        {
                            ReadDelegateCompiler.VertexAttributesReader(subReader, _graphMlNamespace, vertex);
                        }

                    // add to graph
                    VisitedGraph.AddVertex(vertex);
                    vertices.Add(id, vertex);
                }
            }
        }

        #region Compiler

        private delegate void ReadVertexAttributesDelegate(
            XmlReader reader,
            string namespaceUri,
            TVertex v);

        private delegate void ReadEdgeAttributesDelegate(
            XmlReader reader,
            string namespaceUri,
            TEdge e);

        private delegate void ReadGraphAttributesDelegate(
            XmlReader reader,
            string namespaceUri,
            TGraph g);

        private static class ReadDelegateCompiler
        {
            public static readonly ReadVertexAttributesDelegate VertexAttributesReader;

            public static readonly ReadEdgeAttributesDelegate EdgeAttributesReader;

            public static readonly ReadGraphAttributesDelegate GraphAttributesReader;

            public static readonly Action<TVertex> SetVertexDefault;

            public static readonly Action<TEdge> SetEdgeDefault;

            public static readonly Action<TGraph> SetGraphDefault;

            static ReadDelegateCompiler()
            {
                VertexAttributesReader =
                    (ReadVertexAttributesDelegate)CreateReadDelegate(
                        typeof(ReadVertexAttributesDelegate),
                        typeof(TVertex)

                        //,"id"
                    );
                EdgeAttributesReader =
                    (ReadEdgeAttributesDelegate)CreateReadDelegate(
                        typeof(ReadEdgeAttributesDelegate),
                        typeof(TEdge)

                        //,"id", "source", "target"
                    );
                GraphAttributesReader =
                    (ReadGraphAttributesDelegate)CreateReadDelegate(
                        typeof(ReadGraphAttributesDelegate),
                        typeof(TGraph)
                    );
                SetVertexDefault =
                    (Action<TVertex>)CreateSetDefaultDelegate(
                        typeof(Action<TVertex>),
                        typeof(TVertex)
                    );
                SetEdgeDefault =
                    (Action<TEdge>)CreateSetDefaultDelegate(
                        typeof(Action<TEdge>),
                        typeof(TEdge)
                    );
                SetGraphDefault =
                    (Action<TGraph>)CreateSetDefaultDelegate(
                        typeof(Action<TGraph>),
                        typeof(TGraph)
                    );
            }

            public static Delegate CreateReadDelegate(
                Type delegateType,
                Type elementType

                //,params string[] ignoredAttributes
            )
            {
                Contract.Requires(delegateType != null);
                Contract.Requires(elementType != null);

                var method = new DynamicMethod(
                    "Read" + elementType.Name,
                    typeof(void),

                    //          reader, namespaceUri
                    new[] { typeof(XmlReader), typeof(string), elementType },
                    elementType.GetTypeInfo().Module
                );
                var gen = method.GetILGenerator();

                var key = gen.DeclareLocal(typeof(string));

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldstr, "key");
                gen.EmitCall(OpCodes.Callvirt, Metadata.GetAttributeMethod, null);
                gen.Emit(OpCodes.Stloc_0);

                //// if (String.Equals(key, "id")) continue;
                //foreach (string ignoredAttribute in ignoredAttributes)
                //{
                //    gen.Emit(OpCodes.Ldloc_0);
                //    gen.Emit(OpCodes.Ldstr, ignoredAttribute);
                //    gen.EmitCall(OpCodes.Call, Metadata.StringEqualsMethod, null);
                //    gen.Emit(OpCodes.Brtrue, doWhile);
                //}

                // we need to create the swicth for each property
                var next = gen.DefineLabel();
                var @return = gen.DefineLabel();
                var first = true;
                foreach (var kv in SerializationHelper.GetAttributeProperties(elementType))
                {
                    var property = kv.Property;

                    if (!first)
                    {
                        gen.MarkLabel(next);
                        next = gen.DefineLabel();
                    }
                    first = false;

                    // if (!key.Equals("foo"))
                    gen.Emit(OpCodes.Ldloc_0);
                    gen.Emit(OpCodes.Ldstr, kv.Name);
                    gen.EmitCall(OpCodes.Call, Metadata.StringEqualsMethod, null);

                    // if false jump to next
                    gen.Emit(OpCodes.Brfalse, next);

                    // do our stuff
                    MethodInfo readMethod = null;
                    if (!Metadata.TryGetReadContentMethod(property.PropertyType, out readMethod))
                    {
                        throw new ArgumentException(string.Format("Property {0} has a non-supported type", property.Name));
                    }

                    // do we have a set method ?
                    var setMethod = property.GetSetMethod();
                    if (setMethod == null)
                    {
                        throw new ArgumentException(string.Format("Property {0}.{1} has not set method", property.DeclaringType, property.Name));
                    }

                    // reader.ReadXXX
                    gen.Emit(OpCodes.Ldarg_2); // element
                    gen.Emit(OpCodes.Ldarg_0); // reader
                    gen.Emit(OpCodes.Ldstr, "data");
                    gen.Emit(OpCodes.Ldarg_1); // namespace uri
                    gen.EmitCall(OpCodes.Callvirt, readMethod, null);
                    gen.EmitCall(OpCodes.Callvirt, setMethod, null);

                    // jump to do while
                    gen.Emit(OpCodes.Br, @return);
                }

                // we don't know this parameter.. we throw
                gen.MarkLabel(next);
                gen.Emit(OpCodes.Ldloc_0);
                gen.Emit(OpCodes.Newobj, Metadata.ArgumentExceptionCtor);
                gen.Emit(OpCodes.Throw);

                gen.MarkLabel(@return);
                gen.Emit(OpCodes.Ret);

                //let's bake the method
                return method.CreateDelegate(delegateType);
            }

            public static Delegate CreateSetDefaultDelegate(
                Type delegateType,
                Type elementType
            )
            {
                Contract.Requires(delegateType != null);
                Contract.Requires(elementType != null);

                var method = new DynamicMethod(
                    "Set" + elementType.Name + "Default",
                    typeof(void),
                    new[] { elementType },
                    elementType.DeclaringType == null
                        ? elementType
                        : elementType.DeclaringType
                );
                var gen = method.GetILGenerator();

                // we need to create the swicth for each property
                foreach (var kv in SerializationHelper.GetAttributeProperties(elementType))
                {
                    var property = kv.Property;
                    var defaultValueAttribute = property.GetCustomAttribute(typeof(DefaultValueAttribute))
                                                    as DefaultValueAttribute;
                    if (defaultValueAttribute == null)
                    {
                        continue;
                    }
                    var setMethod = property.GetSetMethod();
                    if (setMethod == null)
                    {
                        throw new InvalidOperationException("property " + property.Name + " is not settable");
                    }
                    var value = defaultValueAttribute.Value;
                    if (value != null &&
                        value.GetType() != property.PropertyType)
                    {
                        throw new InvalidOperationException("invalid default value type of property " + property.Name);
                    }
                    gen.Emit(OpCodes.Ldarg_0);
                    switch (Type.GetTypeCode(property.PropertyType))
                    {
                        case TypeCode.Int32:
                            gen.Emit(OpCodes.Ldc_I4, (int)value);
                            break;
                        case TypeCode.Int64:
                            gen.Emit(OpCodes.Ldc_I8, (long)value);
                            break;
                        case TypeCode.Single:
                            gen.Emit(OpCodes.Ldc_R4, (float)value);
                            break;
                        case TypeCode.Double:
                            gen.Emit(OpCodes.Ldc_R8, (double)value);
                            break;
                        case TypeCode.String:
                            gen.Emit(OpCodes.Ldstr, (string)value);
                            break;
                        case TypeCode.Boolean:
                            gen.Emit(
                                (bool)value
                                    ? OpCodes.Ldc_I4_1
                                    : OpCodes.Ldc_I4_0);
                            break;
                        default:
                            throw new InvalidOperationException("unsupported type " + property.PropertyType);
                    }
                    gen.EmitCall(
                        setMethod.IsVirtual
                            ? OpCodes.Callvirt
                            : OpCodes.Call,
                        setMethod,
                        null);
                }
                gen.Emit(OpCodes.Ret);

                //let's bake the method
                return method.CreateDelegate(delegateType);
            }

            private static class Metadata
            {
                public static readonly MethodInfo ReadToFollowingMethod = typeof(XmlReader).GetTypeInfo()
                                                                                           .GetMethod("ReadToFollowing", new[] { typeof(string), typeof(string) }, null);

                public static readonly MethodInfo GetAttributeMethod = typeof(XmlReader).GetTypeInfo().GetMethod("GetAttribute", new[] { typeof(string) }, null);

                public static readonly PropertyInfo NameProperty = typeof(XmlReader).GetTypeInfo().GetProperty("Name");

                public static readonly PropertyInfo NamespaceUriProperty = typeof(XmlReader).GetTypeInfo().GetProperty("NamespaceUri");

                public static readonly MethodInfo StringEqualsMethod = typeof(string).GetTypeInfo().GetMethod("op_Equality", new[] { typeof(string), typeof(string) }, null);

                public static readonly ConstructorInfo ArgumentExceptionCtor = typeof(ArgumentException).GetTypeInfo().GetConstructor(new[] { typeof(string) });

                private static readonly Dictionary<Type, MethodInfo> ReadContentMethods;

                static Metadata()
                {
                    ReadContentMethods = new Dictionary<Type, MethodInfo>();
                    ReadContentMethods.Add(typeof(bool), typeof(XmlReader).GetTypeInfo().GetMethod("ReadElementContentAsBoolean", new[] { typeof(string), typeof(string) }));
                    ReadContentMethods.Add(typeof(int), typeof(XmlReader).GetTypeInfo().GetMethod("ReadElementContentAsInt", new[] { typeof(string), typeof(string) }));
                    ReadContentMethods.Add(typeof(long), typeof(XmlReader).GetTypeInfo().GetMethod("ReadElementContentAsLong", new[] { typeof(string), typeof(string) }));
                    ReadContentMethods.Add(typeof(float), typeof(XmlReader).GetTypeInfo().GetMethod("ReadElementContentAsFloat", new[] { typeof(string), typeof(string) }));
                    ReadContentMethods.Add(typeof(double), typeof(XmlReader).GetTypeInfo().GetMethod("ReadElementContentAsDouble", new[] { typeof(string), typeof(string) }));
                    ReadContentMethods.Add(typeof(string), typeof(XmlReader).GetTypeInfo().GetMethod("ReadElementContentAsString", new[] { typeof(string), typeof(string) }));
                }

                public static bool TryGetReadContentMethod(Type type, out MethodInfo method)
                {
                    Contract.Requires(type != null);

                    var result = ReadContentMethods.TryGetValue(type, out method);
                    Contract.Assert(!result || method != null, type.FullName);
                    return result;
                }
            }
        }

        #endregion
    }
}