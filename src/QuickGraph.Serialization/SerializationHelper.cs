namespace QuickGraph.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Xml.Serialization;

    internal struct PropertySerializationInfo
    {
        public readonly PropertyInfo Property;

        public readonly string Name;

        private readonly object _value;

        private readonly bool _hasValue;

        public PropertySerializationInfo(
            PropertyInfo property,
            string name)
        {
            Property = property;
            Name = name;
            _value = null;
            _hasValue = false;
        }

        public PropertySerializationInfo(
            PropertyInfo property,
            string name,
            object value)
        {
            Property = property;
            Name = name;
            _value = value;
            _hasValue = _value != null;
        }

        public bool TryGetDefaultValue(out object value)
        {
            value = _value;
            return _hasValue;
        }
    }

    internal static class SerializationHelper
    {
        public static IEnumerable<PropertySerializationInfo> GetAttributeProperties(Type type)
        {
            var currentType = type;
            while (
                currentType != null &&
                currentType != typeof(object) &&
                currentType != typeof(ValueType))
            {
                foreach (var property in currentType.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    // must have a get, and not be an index
                    if (!property.CanRead || property.GetIndexParameters().Length > 0)
                    {
                        continue;
                    }

                    // is it tagged with XmlAttributeAttribute?
                    string name;
                    if (TryGetAttributeName(property, out name))
                    {
                        object value;
                        if (TryGetDefaultValue(property, out value))
                        {
                            yield return new PropertySerializationInfo(property, name, value);
                        }
                        else
                        {
                            yield return new PropertySerializationInfo(property, name);
                        }
                    }
                }

                currentType = currentType.GetTypeInfo().BaseType;
            }
        }

        public static bool TryGetAttributeName(PropertyInfo property, out string name)
        {
            var attribute = property.GetCustomAttribute(typeof(XmlAttributeAttribute))
                                as XmlAttributeAttribute;
            if (attribute == null)
            {
                name = null;
                return false;
            }
            if (string.IsNullOrEmpty(attribute.AttributeName))
            {
                name = property.Name;
            }
            else
            {
                name = attribute.AttributeName;
            }
            return true;
        }

        public static bool TryGetDefaultValue(PropertyInfo property, out object value)
        {
            var attribute = property.GetCustomAttribute(typeof(DefaultValueAttribute))
                                as DefaultValueAttribute;
            if (attribute == null)
            {
                value = null;
                return false;
            }
            value = attribute.Value;
            return true;
        }
    }
}