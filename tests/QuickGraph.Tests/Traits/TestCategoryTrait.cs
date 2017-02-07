namespace QuickGraph.Tests.Traits
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class TestCategoryDiscoverer : ITraitDiscoverer
    {
        private const string Key = "Category";

        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            var ctorArgs = traitAttribute.GetConstructorArguments().ToList();
            yield return new KeyValuePair<string, string>(Key, ctorArgs[0].ToString());
        }
    }

    //NOTICE: Take a note that you must provide appropriate namespace here
    [TraitDiscoverer("QuickGraph.Tests.Traits.CategoryDiscoverer", "QuickGraph.Tests")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestCategoryAttribute : Attribute,
                                     ITraitAttribute
    {
        public TestCategoryAttribute(string category)
        {
        }
    }
}