//namespace QuickGraph.Serialization
//{
//    using System.ComponentModel;
//    using System.IO;
//    using System.Linq;
//    using System.Reflection;
//    using System.Xml;

//    using Xunit;

//    public class GraphMlSerializerWithArgumentsTest
//    {
//        [Fact]
//        public void WriteEdge()
//        {
//            {
//                var g = new TestGraph
//                        {
//                            Bool = true,
//                            Double = 1.0,
//                            Float = 2.0F,
//                            Int = 10,
//                            Long = 100,
//                            String = "foo"
//                        };

//                var v1 = new TestVertex("v1")
//                         {
//                             StringDefault = "bla",
//                             String = "string",
//                             Int = 10,
//                             Long = 20,
//                             Float = 25.0F,
//                             Double = 30.0,
//                             Bool = true
//                         };

//                var v2 = new TestVertex("v2")
//                         {
//                             StringDefault = "bla",
//                             String = "string2",
//                             Int = 110,
//                             Long = 120,
//                             Float = 125.0F,
//                             Double = 130.0,
//                             Bool = true
//                         };

//                g.AddVertex(v1);
//                g.AddVertex(v2);

//                var e = new TestEdge(
//                    v1,
//                    v2,
//                    "e1",
//                    "edge",
//                    90,
//                    100,
//                    25.0F,
//                    110.0,
//                    true
//                );
//                g.AddEdge(e);
//                var sg = VerifySerialization(g);

//                var se = sg.Edges.First();
//                Assert.Equal(e.Id, se.Id);
//                Assert.Equal(e.String, se.String);
//                Assert.Equal(e.Int, se.Int);
//                Assert.Equal(e.Long, se.Long);
//                Assert.Equal(e.Float, se.Float);
//                Assert.Equal(e.Double, se.Double);
//                Assert.Equal(e.Bool, se.Bool);
//            }
//        }

//        [Fact]
//        public void WriteVertex()
//        {
//            var g = new TestGraph
//                    {
//                        Bool = true,
//                        Double = 1.0,
//                        Float = 2.0F,
//                        Int = 10,
//                        Long = 100,
//                        String = "foo"
//                    };

//            var v = new TestVertex("v1")
//                    {
//                        StringDefault = "bla",
//                        String = "string",
//                        Int = 10,
//                        Long = 20,
//                        Float = 25.0F,
//                        Double = 30.0,
//                        Bool = true
//                    };

//            g.AddVertex(v);
//            var sg = VerifySerialization(g);
//            Assert.Equal(g.Bool, sg.Bool);
//            Assert.Equal(g.Double, sg.Double);
//            Assert.Equal(g.Float, sg.Float);
//            Assert.Equal(g.Int, sg.Int);
//            Assert.Equal(g.Long, sg.Long);
//            Assert.Equal(g.String, sg.String);

//            var sv = sg.Vertices.First();
//            Assert.Equal(sv.StringDefault, "bla");
//            Assert.Equal(v.String, sv.String);
//            Assert.Equal(v.Int, sv.Int);
//            Assert.Equal(v.Long, sv.Long);
//            Assert.Equal(v.Float, sv.Float);
//            Assert.Equal(v.Double, sv.Double);
//            Assert.Equal(v.Bool, sv.Bool);
//        }

//        private TestGraph VerifySerialization(TestGraph g)
//        {
//            string xml;
//            using (var writer = new StringWriter())
//            {
//                var settins = new XmlWriterSettings();
//                settins.Indent = true;
//                using (var xwriter = XmlWriter.Create(writer, settins))
//                {
//                    g.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(
//                        xwriter,
//                        v => v.ID,
//                        e => e.ID
//                    );
//                }

//                xml = writer.ToString();
//                TestConsole.WriteLine("serialized: " + xml);
//            }

//            TestGraph newg;
//            using (var reader = new StringReader(xml))
//            {
//                newg = new TestGraph();
//                newg.DeserializeAndValidateFromGraphML(
//                    reader,
//                    id => new TestVertex(id),
//                    (source, target, id) => new TestEdge(source, target, id)
//                );
//            }

//            string newxml;
//            using (var writer = new StringWriter())
//            {
//                var settins = new XmlWriterSettings();
//                settins.Indent = true;
//                using (var xwriter = XmlWriter.Create(writer, settins))
//                {
//                    newg.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(
//                        xwriter,
//                        v => v.ID,
//                        e => e.ID);
//                }
//                newxml = writer.ToString();
//                TestConsole.WriteLine("roundtrip: " + newxml);
//            }

//            Assert.Equal(xml, newxml);

//            return newg;
//        }

//        public class Dummy
//        {
//        }

//        public sealed class TestEdge : Edge<TestVertex>
//        {
//            public string Id { get; }

//            [XmlAttribute("e_string")]
//            [DefaultValue("bla")]
//            public string String { get; set; }

//            [XmlAttribute("e_int")]
//            [DefaultValue(1)]
//            public int Int { get; set; }

//            [XmlAttribute("e_long")]
//            [DefaultValue(2L)]
//            public long Long { get; set; }

//            [XmlAttribute("e_double")]
//            public double Double { get; set; }

//            [XmlAttribute("e_bool")]
//            public bool Bool { get; set; }

//            [XmlAttribute("e_float")]
//            public float Float { get; set; }

//            public TestEdge(
//                TestVertex source,
//                TestVertex target,
//                string id)
//                : base(source, target)
//            {
//                Id = id;
//            }

//            public TestEdge(
//                TestVertex source,
//                TestVertex target,
//                string id,
//                string _string,
//                int _int,
//                long _long,
//                float _float,
//                double _double,
//                bool _bool
//            )
//                : this(source, target, id)
//            {
//                String = _string;
//                Int = _int;
//                Long = _long;
//                Float = _float;
//                Double = _double;
//                Bool = _bool;
//            }
//        }

//        public sealed class TestGraph
//            : AdjacencyGraph<TestVertex, TestEdge>
//        {
//            private bool _bool;

//            private double _double;

//            private float _float;

//            private int _int;

//            private long _long;

//            private string _string;

//            [XmlAttribute("g_string")]
//            [DefaultValue("bla")]
//            public string String
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _string;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _string = value;
//                }
//            }

//            [XmlAttribute("g_int")]
//            [DefaultValue(1)]
//            public int Int
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _int;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _int = value;
//                }
//            }

//            [XmlAttribute("g_long")]
//            [DefaultValue(2L)]
//            public long Long
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _long;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _long = value;
//                }
//            }

//            [XmlAttribute("g_bool")]
//            public bool Bool
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _bool;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _bool = value;
//                }
//            }

//            [XmlAttribute("g_float")]
//            public float Float
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _float;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _float = value;
//                }
//            }

//            [XmlAttribute("g_double")]
//            public double Double
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _double;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _double = value;
//                }
//            }
//        }

//        public sealed class TestVertex
//        {
//            private bool _bool;

//            private double _double;

//            private float _float;

//            private int _int;

//            private long _long;

//            private string _string;

//            private string _stringd;

//            public string Id { get; }

//            [XmlAttribute("v_stringdefault")]
//            [DefaultValue("bla")]
//            public string StringDefault
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _stringd;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _stringd = value;
//                }
//            }

//            [XmlAttribute("v_string")]
//            [DefaultValue("bla")]
//            public string String
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _string;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _string = value;
//                }
//            }

//            [XmlAttribute("v_int")]
//            [DefaultValue(1)]
//            public int Int
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _int;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _int = value;
//                }
//            }

//            [XmlAttribute("v_long")]
//            [DefaultValue(2L)]
//            public long Long
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _long;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _long = value;
//                }
//            }

//            [XmlAttribute("v_bool")]
//            public bool Bool
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _bool;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _bool = value;
//                }
//            }

//            [XmlAttribute("v_float")]
//            public float Float
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _float;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _float = value;
//                }
//            }

//            [XmlAttribute("v_double")]
//            public double Double
//            {
//                get
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    return _double;
//                }
//                set
//                {
//                    TestConsole.WriteLine(MethodInfo.GetCurrentMethod());
//                    _double = value;
//                }
//            }

//            public TestVertex(
//                string id)
//            {
//                Id = id;
//            }
//        }
//    }
//}