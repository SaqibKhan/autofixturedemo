using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AutofixtureWorkshop
{
    public class FrozenAttributeTests
    {
        [Theory, AutoData]
        public void InjectsUniqueStrings(string val1, string val2, string val3)
        {
            val1.Should().NotMatch(val2);
            val2.Should().NotMatch(val3);
            val3.Should().NotMatch(val1);
        }

        [Theory, AutoData]
        public void InjectsSameStrings(string val1, [Frozen]string val2, string val3)
        {
            val1.Should().NotMatch(val2);
            val2.Should().Match(val3);
            val3.Should().NotMatch(val1);
        }

        [Theory, AutoData]
        public void InjectsStringToComplexType([Frozen]string val1, ComplexType type)
        {
            type.StringProperty.Should().Be(val1);
            type.StringProperty2.Should().Be(val1);
        }

        [Theory, AutoData]
        public void InjectsStringToComplexTypeProperty2([Frozen(Matching.MemberName)]string stringProperty2, ComplexType type)
        {
            type.StringProperty.Should().NotBe(stringProperty2);
            type.StringProperty2.Should().Be(stringProperty2);
        }

        [Theory, AutoData]
        public void InjectsComplexDependency([Frozen(Matching.DirectBaseType)]Dependency dep, ComplexType type)
        {
            type.Dependency.Should().Be(dep);
        }

        public class DependencyBase
        {
            
        }

        public class Dependency : DependencyBase
        {
        }

        public class ComplexType
        {
            public string StringProperty { get; set; }
            public string StringProperty2 { get; set; }
            public int IntProperty { get; set; }

            public DependencyBase Dependency { get; set; }

            public string ReadOnlyProperty { get; private set; }

            public void Method(string value)
            {
                ReadOnlyProperty = value;
            }
        }
    }
}
