namespace QuickGraph.Serialization
{
    /// <summary>A resolver that loads graphml DTD and XSD schemas from embedded resources.</summary>
    public sealed class GraphMlXmlResolver

        //: XmlResolver
    {
        //private readonly XmlResolver _baseResolver;

        //public GraphMlXmlResolver()
        //    : this(new XmlUrlResolver())
        //{
        //}
        //public GraphMlXmlResolver(XmlResolver baseResolver)
        //{
        //    Contract.Requires(baseResolver != null);

        //    this._baseResolver = baseResolver;
        //}

        public const string GraphMlNamespace = "http://graphml.graphdrawing.org/xmlns";

        //private ICredentials _credentials;
        //public override ICredentials Credentials
        //{
        //    set
        //    {
        //        this._credentials = value;
        //    }
        //}

        //public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        //{
        //    if (absoluteUri.AbsoluteUri == "http://www.graphdrawing.org/dtds/graphml.dtd")
        //        return typeof(GraphMlExtensions).Assembly.GetManifestResourceStream(typeof(GraphMlExtensions), "graphml.dtd");
        //    else if (absoluteUri.AbsoluteUri == "http://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd")
        //        return typeof(GraphMlExtensions).Assembly.GetManifestResourceStream(typeof(GraphMlExtensions), "graphml.xsd");
        //    else if (absoluteUri.AbsoluteUri == "http://graphml.graphdrawing.org/xmlns/1.0/graphml-structure.xsd")
        //        return typeof(GraphExtensions).Assembly.GetManifestResourceStream(typeof(GraphMlExtensions), "graphml-structure.xsd");

        //    return this._baseResolver.GetEntity(absoluteUri, role, ofObjectToReturn);
        //}
    }
}