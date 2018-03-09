using System;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Xunit;

namespace AutofixtureWorkshop
{
    public class IdiomaticTests
    {
        [Theory, AutoData]
        public void GuardsInputParametersFromNulls(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(MethodGuard));
        }

        [Theory, AutoData]
        public void ChecksCtorInit(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(ConstructorProperty));
        }

        [Theory, AutoData]
        public void EnsuresSameValueInProperty(WritablePropertyAssertion assertion)
        {
            assertion.Verify(typeof(PropertySetter));
        }

        public class ConstructorProperty
        {
            public string Property1 { get; }

            public ConstructorProperty(string property1)
            {
                Property1 = property1;
            }
        }

        public class PropertySetter
        {
            private string property1;

            public string Property1
            {
                get { return property1; }
                set { property1 = value; }
            }
        }

        public class MethodGuard
        {
            public void Method(string val1)
            {
                if (string.IsNullOrEmpty(val1))
                {
                    throw new ArgumentNullException(nameof(val1));
                }
            }
        }
    }
}
