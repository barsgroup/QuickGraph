namespace QuickGraph.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Xml;

    /// <summary>A GraphML ( http://graphml.graphdrawing.org/ ) format serializer.</summary>
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
    public sealed class GraphMlSerializer<TVertex, TEdge, TGraph>
        : SerializerBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IEdgeListGraph<TVertex, TEdge>
    {
        public void Serialize(
            XmlWriter writer,
            TGraph visitedGraph,
            VertexIdentity<TVertex> vertexIdentities,
            EdgeIdentity<TVertex, TEdge> edgeIdentities
        )
        {
            Contract.Requires(writer != null);
            Contract.Requires(visitedGraph != null);
            Contract.Requires(vertexIdentities != null);
            Contract.Requires(edgeIdentities != null);

            var worker = new WriterWorker(this, writer, visitedGraph, vertexIdentities, edgeIdentities);
            worker.Serialize();
        }

        internal class WriterWorker
        {
            private readonly EdgeIdentity<TVertex, TEdge> _edgeIdentities;

            private readonly VertexIdentity<TVertex> _vertexIdentities;

            public GraphMlSerializer<TVertex, TEdge, TGraph> Serializer { get; }

            public XmlWriter Writer { get; }

            public TGraph VisitedGraph { get; }

            public WriterWorker(
                GraphMlSerializer<TVertex, TEdge, TGraph> serializer,
                XmlWriter writer,
                TGraph visitedGraph,
                VertexIdentity<TVertex> vertexIdentities,
                EdgeIdentity<TVertex, TEdge> edgeIdentities)
            {
                Contract.Requires(serializer != null);
                Contract.Requires(writer != null);
                Contract.Requires(visitedGraph != null);
                Contract.Requires(vertexIdentities != null);
                Contract.Requires(edgeIdentities != null);

                Serializer = serializer;
                Writer = writer;
                VisitedGraph = visitedGraph;
                _vertexIdentities = vertexIdentities;
                _edgeIdentities = edgeIdentities;
            }

            public void Serialize()
            {
                WriteHeader();
                WriteGraphAttributeDefinitions();
                WriteVertexAttributeDefinitions();
                WriteEdgeAttributeDefinitions();
                WriteGraphHeader();
                WriteVertices();
                WriteEdges();
                WriteGraphFooter();
                WriteFooter();
            }

            private void WriteAttributeDefinitions(string forNode, Type nodeType)
            {
                Contract.Requires(forNode != null);
                Contract.Requires(nodeType != null);

                foreach (var kv in SerializationHelper.GetAttributeProperties(nodeType))
                {
                    var property = kv.Property;
                    var name = kv.Name;
                    var propertyType = property.PropertyType;

                    //<key id="d1" for="edge" attr.name="weight" attr.type="double"/>
                    Writer.WriteStartElement("key", GraphMlXmlResolver.GraphMlNamespace);
                    Writer.WriteAttributeString("id", name);
                    Writer.WriteAttributeString("for", forNode);
                    Writer.WriteAttributeString("attr.name", name);

                    switch (Type.GetTypeCode(propertyType))
                    {
                        case TypeCode.Boolean:
                            Writer.WriteAttributeString("attr.type", "boolean");
                            break;
                        case TypeCode.Int32:
                            Writer.WriteAttributeString("attr.type", "int");
                            break;
                        case TypeCode.Int64:
                            Writer.WriteAttributeString("attr.type", "long");
                            break;
                        case TypeCode.Single:
                            Writer.WriteAttributeString("attr.type", "float");
                            break;
                        case TypeCode.Double:
                            Writer.WriteAttributeString("attr.type", "double");
                            break;
                        case TypeCode.String:
                            Writer.WriteAttributeString("attr.type", "string");
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Property type {0}.{1} not supported by the GraphML schema", property.DeclaringType, property.Name));
                    }

                    // <default>...</default>
                    object defaultValue;
                    if (kv.TryGetDefaultValue(out defaultValue))
                    {
                        Writer.WriteStartElement("default");
                        switch (Type.GetTypeCode(defaultValue.GetType()))
                        {
                            case TypeCode.Boolean:
                                Writer.WriteString(XmlConvert.ToString((bool)defaultValue));
                                break;
                            case TypeCode.Int32:
                                Writer.WriteString(XmlConvert.ToString((int)defaultValue));
                                break;
                            case TypeCode.Int64:
                                Writer.WriteString(XmlConvert.ToString((long)defaultValue));
                                break;
                            case TypeCode.Single:
                                Writer.WriteString(XmlConvert.ToString((float)defaultValue));
                                break;
                            case TypeCode.Double:
                                Writer.WriteString(XmlConvert.ToString((double)defaultValue));
                                break;
                            case TypeCode.String:
                                Writer.WriteString((string)defaultValue);
                                break;
                            default:
                                throw new NotSupportedException(string.Format("Property type {0}.{1} not supported by the GraphML schema", property.DeclaringType, property.Name));
                        }
                        Writer.WriteEndElement();
                    }

                    Writer.WriteEndElement();
                }
            }

            private void WriteEdgeAttributeDefinitions()
            {
                var forNode = "edge";
                var nodeType = typeof(TEdge);

                WriteAttributeDefinitions(forNode, nodeType);
            }

            private void WriteEdges()
            {
                foreach (var e in VisitedGraph.Edges)
                {
                    Writer.WriteStartElement("edge", GraphMlXmlResolver.GraphMlNamespace);
                    Writer.WriteAttributeString("id", _edgeIdentities(e));
                    Writer.WriteAttributeString("source", _vertexIdentities(e.Source));
                    Writer.WriteAttributeString("target", _vertexIdentities(e.Target));
                    WriteDelegateCompiler.EdgeAttributesWriter(Writer, e);
                    Writer.WriteEndElement();
                }
            }

            private void WriteFooter()
            {
                Writer.WriteEndElement();
                Writer.WriteEndDocument();
            }

            private void WriteGraphAttributeDefinitions()
            {
                var forNode = "graph";
                var nodeType = typeof(TGraph);

                WriteAttributeDefinitions(forNode, nodeType);
            }

            private void WriteGraphFooter()
            {
                Writer.WriteEndElement();
            }

            private void WriteGraphHeader()
            {
                Writer.WriteStartElement("graph", GraphMlXmlResolver.GraphMlNamespace);
                Writer.WriteAttributeString("id", "G");
                Writer.WriteAttributeString(
                    "edgedefault",
                    VisitedGraph.IsDirected
                        ? "directed"
                        : "undirected"
                );
                Writer.WriteAttributeString("parse.nodes", VisitedGraph.VertexCount.ToString());
                Writer.WriteAttributeString("parse.edges", VisitedGraph.EdgeCount.ToString());
                Writer.WriteAttributeString("parse.order", "nodesfirst");
                Writer.WriteAttributeString("parse.nodeids", "free");
                Writer.WriteAttributeString("parse.edgeids", "free");

                WriteDelegateCompiler.GraphAttributesWriter(Writer, VisitedGraph);
            }

            private void WriteHeader()
            {
                if (Serializer.EmitDocumentDeclaration)
                {
                    Writer.WriteStartDocument();
                }
                Writer.WriteStartElement("", "graphml", GraphMlXmlResolver.GraphMlNamespace);
            }

            private void WriteVertexAttributeDefinitions()
            {
                var forNode = "node";
                var nodeType = typeof(TVertex);

                WriteAttributeDefinitions(forNode, nodeType);
            }

            private void WriteVertices()
            {
                foreach (var v in VisitedGraph.Vertices)
                {
                    Writer.WriteStartElement("node", GraphMlXmlResolver.GraphMlNamespace);
                    Writer.WriteAttributeString("id", _vertexIdentities(v));
                    WriteDelegateCompiler.VertexAttributesWriter(Writer, v);
                    Writer.WriteEndElement();
                }
            }
        }

        #region Compiler

        private delegate void WriteVertexAttributesDelegate(
            XmlWriter writer,
            TVertex v);

        private delegate void WriteEdgeAttributesDelegate(
            XmlWriter writer,
            TEdge e);

        private delegate void WriteGraphAttributesDelegate(
            XmlWriter writer,
            TGraph e);

        public static bool MoveNextData(XmlReader reader)
        {
            Contract.Requires(reader != null);
            return
                reader.NodeType == XmlNodeType.Element &&
                reader.Name == "data" &&
                reader.NamespaceURI == GraphMlXmlResolver.GraphMlNamespace;
        }

        private static class WriteDelegateCompiler
        {
            public static readonly WriteVertexAttributesDelegate VertexAttributesWriter;

            public static readonly WriteEdgeAttributesDelegate EdgeAttributesWriter;

            public static readonly WriteGraphAttributesDelegate GraphAttributesWriter;

            static WriteDelegateCompiler()
            {
                VertexAttributesWriter =
                    (WriteVertexAttributesDelegate)CreateWriteDelegate(
                        typeof(TVertex),
                        typeof(WriteVertexAttributesDelegate));
                EdgeAttributesWriter =
                    (WriteEdgeAttributesDelegate)CreateWriteDelegate(
                        typeof(TEdge),
                        typeof(WriteEdgeAttributesDelegate)
                    );
                GraphAttributesWriter =
                    (WriteGraphAttributesDelegate)CreateWriteDelegate(
                        typeof(TGraph),
                        typeof(WriteGraphAttributesDelegate)
                    );
            }

            public static Delegate CreateWriteDelegate(Type nodeType, Type delegateType)
            {
                Contract.Requires(nodeType != null);
                Contract.Requires(delegateType != null);

                var method = new DynamicMethod(
                    "Write" + delegateType.Name + nodeType.Name,
                    typeof(void),
                    new[] { typeof(XmlWriter), nodeType },
                    nodeType.GetTypeInfo().Module
                );
                var gen = method.GetILGenerator();
                var @default = default(Label);

                foreach (var kv in SerializationHelper.GetAttributeProperties(nodeType))
                {
                    var property = kv.Property;
                    var name = kv.Name;

                    var getMethod = property.GetGetMethod();
                    if (getMethod == null)
                    {
                        throw new NotSupportedException(string.Format("Property {0}.{1} has not getter", property.DeclaringType, property.Name));
                    }
                    MethodInfo writeValueMethod;
                    if (!Metadata.TryGetWriteValueMethod(property.PropertyType, out writeValueMethod))
                    {
                        throw new NotSupportedException(string.Format("Property {0}.{1} type is not supported", property.DeclaringType, property.Name));
                    }

                    var defaultValueAttribute = property.GetCustomAttribute(typeof(DefaultValueAttribute)) as DefaultValueAttribute;
                    if (defaultValueAttribute != null)
                    {
                        @default = gen.DefineLabel();
                        var value = defaultValueAttribute.Value;
                        if (value != null &&
                            value.GetType() != property.PropertyType)
                        {
                            throw new InvalidOperationException("inconsistent default value of property " + property.Name);
                        }

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
                        gen.Emit(OpCodes.Ldarg_1);
                        gen.EmitCall(OpCodes.Callvirt, getMethod, null);
                        gen.Emit(OpCodes.Ceq);
                        gen.Emit(OpCodes.Brtrue, @default);
                    }

                    // for each property of the type,
                    // write it to the xmlwriter (we need to take care of value types, etc...)
                    // writer.WriteStartElement("data")
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldstr, "data");
                    gen.Emit(OpCodes.Ldstr, GraphMlXmlResolver.GraphMlNamespace);
                    gen.EmitCall(OpCodes.Callvirt, Metadata.WriteStartElementMethod, null);

                    // writer.WriteStartAttribute("key");
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldstr, "key");
                    gen.Emit(OpCodes.Ldstr, name);
                    gen.EmitCall(OpCodes.Callvirt, Metadata.WriteAttributeStringMethod, null);

                    // writer.WriteValue(v.xxx);
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldarg_1);
                    gen.EmitCall(OpCodes.Callvirt, getMethod, null);
                    gen.EmitCall(OpCodes.Callvirt, writeValueMethod, null);

                    // writer.WriteEndElement()
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.EmitCall(OpCodes.Callvirt, Metadata.WriteEndElementMethod, null);

                    if (defaultValueAttribute != null)
                    {
                        gen.MarkLabel(@default);
                        @default = default(Label);
                    }
                }

                gen.Emit(OpCodes.Ret);

                //let's bake the method
                return method.CreateDelegate(delegateType);
            }

            private static class Metadata
            {
                public static readonly MethodInfo WriteStartElementMethod =
                    typeof(XmlWriter).GetTypeInfo().GetMethod("WriteStartElement", new[] { typeof(string), typeof(string) });

                public static readonly MethodInfo WriteEndElementMethod = typeof(XmlWriter).GetTypeInfo().GetMethod("WriteEndElement", BindingFlags.Instance | BindingFlags.Public);

                public static readonly MethodInfo WriteStringMethod = typeof(XmlWriter).GetTypeInfo().GetMethod("WriteString", new[] { typeof(string) });

                public static readonly MethodInfo WriteAttributeStringMethod = typeof(XmlWriter).GetTypeInfo()
                                                                                                .GetMethod("WriteAttributeString", new[] { typeof(string), typeof(string) });

                public static readonly MethodInfo WriteStartAttributeMethod = typeof(XmlWriter).GetTypeInfo().GetMethod("WriteStartAttribute", new[] { typeof(string) });

                public static readonly MethodInfo WriteEndAttributeMethod = typeof(XmlWriter).GetTypeInfo()
                                                                                             .GetMethod("WriteEndAttribute", BindingFlags.Instance | BindingFlags.Public);

                private static readonly Dictionary<Type, MethodInfo> WriteValueMethods = new Dictionary<Type, MethodInfo>();

                static Metadata()
                {
                    var writer = typeof(XmlWriter);
                    WriteValueMethods.Add(typeof(bool), writer.GetTypeInfo().GetMethod("WriteValue", new[] { typeof(bool) }));
                    WriteValueMethods.Add(typeof(int), writer.GetTypeInfo().GetMethod("WriteValue", new[] { typeof(int) }));
                    WriteValueMethods.Add(typeof(long), writer.GetTypeInfo().GetMethod("WriteValue", new[] { typeof(long) }));
                    WriteValueMethods.Add(typeof(float), writer.GetTypeInfo().GetMethod("WriteValue", new[] { typeof(float) }));
                    WriteValueMethods.Add(typeof(double), writer.GetTypeInfo().GetMethod("WriteValue", new[] { typeof(double) }));
                    WriteValueMethods.Add(typeof(string), writer.GetTypeInfo().GetMethod("WriteString", new[] { typeof(string) }));
                }

                public static bool TryGetWriteValueMethod(Type valueType, out MethodInfo method)
                {
                    Contract.Requires(valueType != null);
                    return WriteValueMethods.TryGetValue(valueType, out method);
                }
            }
        }

        #endregion
    }
}