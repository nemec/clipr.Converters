using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace clipr.Converters.UnitTests
{
    [TestClass]
    public class ConverterFinderFieldUnitTest
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
            #pragma warning disable 649
            public int SomeValue;

            [TypeConverter(typeof(CustomIntConverter))]
            public int SomeCustomValue;

            public ClassWithDescriptor OtherValue;

            [TypeConverter(typeof(CustomClassWithDescriptorConverter))]
            public ClassWithDescriptor AnotherValue;
            #pragma warning restore 649
        }

        [TestMethod]
        public void GetConverterForField_WithNoCustomConverter_ReturnsBaseConverter()
        {
            var expectedType = typeof(Int32Converter);
            var actual = ConverterFinder
                .GetConverterForField(
                    typeof(TestPropertyClass)
                    .GetField("SomeValue"));

            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void GetConverterForField_WithConverterAppliedToProperty_ReturnsCustomConverter()
        {
            var expectedType = typeof(CustomIntConverter);

            var actual = ConverterFinder
                .GetConverterForField(
                    typeof(TestPropertyClass)
                    .GetField("SomeCustomValue"));
            
            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void GetConverterForField_WithObjectInheritingICustomTypeDescriptor_ReturnsCustomConverter()
        {
            var obj = new ClassWithDescriptor(10);
            var expectedType = typeof(ClassWithDescriptorConverter);

            var actual = ConverterFinder
                .GetConverterForField(
                    typeof(TestPropertyClass)
                    .GetField("OtherValue"),
                    obj);

            Assert.IsInstanceOfType(actual, expectedType);
        }

        [TestMethod]
        public void GetConverterForField_WithNoDefaultConverterAndCustomConverterAppliedToProperty_ReturnsCustomConverter()
        {
            var expectedType = typeof(CustomClassWithDescriptorConverter);

            var actual = ConverterFinder
                .GetConverterForField(
                    typeof(TestPropertyClass)
                    .GetField("AnotherValue"));

            Assert.IsInstanceOfType(actual, expectedType);
        }
    }
}
