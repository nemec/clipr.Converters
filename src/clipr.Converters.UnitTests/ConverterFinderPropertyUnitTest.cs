using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace clipr.Converters.UnitTests
{
    [TestClass]
    public class ConverterFinderPropertyUnitTest
    {
        private class CustomIntConverter : TypeConverter
        {
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var val = Int32.Parse((string)value);
                return val + 10;
            }
        }

        private class ClassWithDescriptorConverter : TypeConverter
        {
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var val = Int32.Parse((string)value);
                return new ClassWithDescriptor(val);
            }
        }

        private class CustomClassWithDescriptorConverter : TypeConverter
        {
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var val = Int32.Parse((string)value);
                return new ClassWithDescriptor(val);
            }
        }

        private class ClassWithDescriptor : ICustomTypeDescriptor
        {
            public int InnerValue { get; private set; }

            public ClassWithDescriptor(int value)
            {
                InnerValue = value;
            }

            public TypeConverter GetConverter()
            {
                return new ClassWithDescriptorConverter();
            }

            #region NotImplemented

            public AttributeCollection GetAttributes()
            {
                throw new NotImplementedException();
            }

            public string GetClassName()
            {
                throw new NotImplementedException();
            }

            public string GetComponentName()
            {
                throw new NotImplementedException();
            }

            public EventDescriptor GetDefaultEvent()
            {
                throw new NotImplementedException();
            }

            public PropertyDescriptor GetDefaultProperty()
            {
                throw new NotImplementedException();
            }

            public object GetEditor(Type editorBaseType)
            {
                throw new NotImplementedException();
            }

            public EventDescriptorCollection GetEvents()
            {
                throw new NotImplementedException();
            }

            public EventDescriptorCollection GetEvents(Attribute[] attributes)
            {
                throw new NotImplementedException();
            }

            public PropertyDescriptorCollection GetProperties()
            {
                throw new NotImplementedException();
            }

            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                throw new NotImplementedException();
            }

            public object GetPropertyOwner(PropertyDescriptor pd)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        private class TestPropertyClass
        {
            public int SomeValue { get; set; }

            [TypeConverter(typeof(CustomIntConverter))]
            public int SomeCustomValue { get; set; }

            public ClassWithDescriptor OtherValue { get; set; }

            [TypeConverter(typeof(CustomClassWithDescriptorConverter))]
            public ClassWithDescriptor AnotherValue { get; set; }
        }

        [TestMethod]
        public void GetConverterForProperty_WithNoCustomConverter_ReturnsBaseConverter()
        {
            var expectedType = typeof(Int32Converter);
            var actual = ConverterFinder
                .GetConverterForProperty(
                    typeof(TestPropertyClass)
                    .GetProperty("SomeValue"));

            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void GetConverterForProperty_WithConverterAppliedToProperty_ReturnsCustomConverter()
        {
            var expectedType = typeof(CustomIntConverter);

            var actual = ConverterFinder
                .GetConverterForProperty(
                    typeof(TestPropertyClass)
                    .GetProperty("SomeCustomValue"));
            
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void GetConverterForProperty_WithObjectInheritingICustomTypeDescriptor_ReturnsCustomConverter()
        {
            var obj = new ClassWithDescriptor(10);
            var expectedType = typeof(ClassWithDescriptorConverter);

            var actual = ConverterFinder
                .GetConverterForProperty(
                    typeof(TestPropertyClass)
                    .GetProperty("OtherValue"),
                    obj);

            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void GetConverterForProperty_WithNoDefaultConverterAndCustomConverterAppliedToProperty_ReturnsCustomConverter()
        {
            var expectedType = typeof(CustomClassWithDescriptorConverter);

            var actual = ConverterFinder
                .GetConverterForProperty(
                    typeof(TestPropertyClass)
                    .GetProperty("AnotherValue"));

            Assert.IsInstanceOfType(actual, expectedType);
        }
    }
}
