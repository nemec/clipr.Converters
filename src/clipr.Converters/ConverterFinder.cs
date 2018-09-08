using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace clipr.Converters
{
    public static class ConverterFinder
    {
        public static TypeConverter GetConverterForProperty(PropertyInfo prop)
        {
            return GetConverterForProperty(prop, null);
        }

        public static TypeConverter GetConverterForProperty(PropertyInfo prop, object declaringInstance)
        {
            var typeAttr = prop.GetCustomAttribute<TypeConverterAttribute>();
            if (typeAttr != null)
            {
                var type = Type.GetType(typeAttr.ConverterTypeName);
                if (type != null)
                {
                    return (TypeConverter)Activator.CreateInstance(type);
                }
            }
            if(declaringInstance != null)
            {
                return TypeDescriptor.GetConverter(declaringInstance);
            }
            return TypeDescriptor.GetConverter(prop.PropertyType);
        }

        public static TypeConverter GetConverterForField(FieldInfo field)
        {
            return GetConverterForField(field, null);
        }

        public static TypeConverter GetConverterForField(FieldInfo field, object declaringInstance)
        {
            var typeAttr = field.GetCustomAttribute<TypeConverterAttribute>();
            if (typeAttr != null)
            {
                var type = Type.GetType(typeAttr.ConverterTypeName);
                if (type != null)
                {
                    return (TypeConverter)Activator.CreateInstance(type);
                }
            }
            if (declaringInstance != null)
            {
                return TypeDescriptor.GetConverter(declaringInstance);
            }
            return TypeDescriptor.GetConverter(field.FieldType);
        }
    }
}
