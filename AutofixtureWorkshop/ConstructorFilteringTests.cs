using System.Collections.Generic;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AutofixtureWorkshop
{
    public class ConstructorFilteringTests
    {
        [Theory, AutoData]
        public void CreatesFromDefaultConstructor(ConstructorFiltering sut)
        {
            sut.DefaultCtorCalled.Should().BeTrue();
        }

        [Theory, AutoData]
        public void CreatesFromFattestConstructor([Greedy]ConstructorFiltering sut)
        {
            sut.FattestCtorCalled.Should().BeTrue();
        }

        [Theory, AutoData]
        public void CreatesFromListConstructor([FavorLists]ConstructorFiltering sut)
        {
            sut.ListCtorCalled.Should().BeTrue();
        }

        [Theory, AutoData]
        public void CreatesFromEnumerablesConstructor([FavorEnumerables]ConstructorFiltering sut)
        {
            sut.SequenceCtorCalled.Should().BeTrue();
        }

        [Theory, AutoData]
        public void CreatesFromArrayConstructor([FavorArrays]ConstructorFiltering sut)
        {
            sut.ArrayCtorCalled.Should().BeTrue();
        }

        public class ConstructorFiltering
        {
            public ConstructorFiltering()
            {
                this.DefaultCtorCalled = true;
            }

            public bool DefaultCtorCalled { get; }

            public ConstructorFiltering(string val1, string val2, string val3)
            {
                this.FattestCtorCalled = true;
            }

            public bool FattestCtorCalled { get; }

            public ConstructorFiltering(IList<string> val1)
            {
                this.ListCtorCalled = true;
            }

            public bool ListCtorCalled { get; }

            public ConstructorFiltering(IEnumerable<string> val1)
            {
                this.SequenceCtorCalled = true;
            }

            public bool SequenceCtorCalled { get; }

            public ConstructorFiltering(params string[] val1)
            {
                this.ArrayCtorCalled = true;
            }

            public bool ArrayCtorCalled { get; }
        }
    }
}
