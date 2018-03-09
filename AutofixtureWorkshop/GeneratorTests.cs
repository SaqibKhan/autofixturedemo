using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AutofixtureWorkshop
{
    public class GeneratorTests
    {
        [Theory, AutoData]
        public void GeneratesRandomNumberOfParameters(int paramCount, Generator<string> generator, ParamsExample sut)
        {
            var vals = generator.Take(paramCount).ToArray();
            sut.Method(vals);
            sut.Count.Should().Be(paramCount);
        }
    }

    public class ParamsExample
    {
        public int Count { get; private set; }

        public void Method(params string[] vals)
        {
            Count = vals.Length;
        }
    }
}
