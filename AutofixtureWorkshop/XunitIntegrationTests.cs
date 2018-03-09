using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AutofixtureWorkshop
{
    public class XunitIntegrationTests
    {
        [Theory, AutoData]
        public void InjectsRandomString(string randomString)
        {
            Console.WriteLine(randomString);
        }

        [Theory, AutoData]
        public void InjectsRandomComplexType(ComplexType sut)
        {
            Console.WriteLine(sut.IntProperty + " " + sut.StringProperty);
        }

        [Theory, CustomAutoData]
        public void InjectsCustomComplexType(ComplexType sut)
        {
            Console.WriteLine(sut.IntProperty + " " + sut.StringProperty);
            sut.ReadOnlyProperty.Should().NotBeNullOrEmpty();
        }

        [Theory, ConventionAutoData]
        public void InjectsComplexTypeByConvention(ComplexType complex, string val1)
        {
            complex.StringProperty2.Should().Be("SpecialValue");
            complex.StringProperty.Should().BeNull();
            Console.WriteLine(val1);
        }

        [Theory]
        [InlineAutoData(5, "val123")]
        [InlineAutoData(56, "8908")]
        public void UsesInlineData(int val1, string val2, ComplexType cplType)
        {
            Console.WriteLine(val1 + " " + val2 + " " + cplType.StringProperty);
        }

        [Theory, ClassData(typeof(MyTestData))]
        public void UsesClassData(int val1, string val2, ComplexType cplType)
        {
            Console.WriteLine(val1 + " " + val2 + " " + cplType.StringProperty);
        }

        public class MyTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                yield return new object[]{6, "123", fixture.Create<ComplexType>()};
                yield return new object[]{9, "987", fixture.Create<ComplexType>()};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class CustomAutoDataAttribute : AutoDataAttribute
        {
            public CustomAutoDataAttribute()
                : base(() => new Fixture().Customize(new MyCustomization()))
            {

            }
        }

        public class ConventionAutoDataAttribute : AutoDataAttribute
        {
            public ConventionAutoDataAttribute()
                : base(() =>
                {
                    var fixture = new Fixture();
                    fixture.Customizations.Add(new MySpecimen());
                    return fixture;
                })
            {

            }

            public class MySpecimen : ISpecimenBuilder
            {
                public object Create(object request, ISpecimenContext context)
                {
                    var pi = request as ParameterInfo;
                    if (pi == null || pi.Name != "complex" || pi.ParameterType != typeof(ComplexType))
                    {
                        return new NoSpecimen();
                    }

                    return new ComplexType { StringProperty2 = "SpecialValue" };
                }
            }
        }

        public class ComplexType
        {
            public string StringProperty { get; set; }
            public string StringProperty2 { get; set; }
            public int IntProperty { get; set; }

            public string ReadOnlyProperty { get; private set; }

            public void Method(string value)
            {
                ReadOnlyProperty = value;
            }
        }

        public class MyCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<ComplexType>(c => c.Do(f => f.Method(fixture.Create<string>())));
            }
        }
    }
}
