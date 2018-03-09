using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace AutofixtureWorkshop
{
    public class FixtureTests
    {
        [Fact]
        public void CreatesRandomString()
        {
            var fixture = new Fixture();
            var result = fixture.Create<string>();
            Console.WriteLine(result);
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void CreatesRandomInt()
        {
            var fixture = new Fixture();
            var res = fixture.Create<int>();
            Console.WriteLine(res);
            res.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CreatesRandomComplexType()
        {
            var fixture = new Fixture();
            var res = fixture.Create<ComplexType>();
            Console.WriteLine(res.IntProperty + " " + res.StringProperty);
        }

        [Fact]
        public void CreatesStringWithCustomAlgorithm()
        {
            var fixture = new Fixture();
            var res = fixture.Build<string>().FromFactory(() => Guid.NewGuid().ToString().Substring(0, 4)).Create();
            Console.WriteLine(res);
        }

        [Fact]
        public void CreateSequenceOfStrings()
        {
            var fixture = new Fixture();
            var res = fixture.CreateMany<string>();
            res.Count().Should().Be(3);
        }

        [Fact]
        public void CreatesListOfStrings()
        {
            var fixture = new Fixture();
            var res = fixture.Create<List<string>>();

            res.Count.Should().Be(3);
        }

        [Fact]
        public void AddsRandomValuesIntoExistingList()
        {
            var list = new List<string>();
            var fixture = new Fixture();
            fixture.AddManyTo(list);
            fixture.AddManyTo(list);
            list.Count.Should().Be(6);
        }

        [Fact]
        public void CreatesSequenceWithFiveRandomValues()
        {
            var fixture = new Fixture();
            fixture.RepeatCount = 5;
            var res = fixture.CreateMany<string>();
            res.Count().Should().Be(5);
        }

        [Fact]
        public void CreatesComplexTypeWithoutValueINStringProperty2()
        {
            var fixture = new Fixture();
            var res = fixture.Build<ComplexType>().Without(c => c.StringProperty2).Create();
            res.StringProperty2.Should().BeNull();
        }

        [Fact]
        public void CreatesComplexTypeWithCustomValueINStringProperty2()
        {
            var fixture = new Fixture();
            var res = fixture.Build<ComplexType>().With(c => c.StringProperty2, "MyValue").Create();
            res.StringProperty2.Should().Be("MyValue");
        }

        [Fact]
        public void OmitsAllPropertiesOnComplexType()
        {
            var fixture = new Fixture();
            var res = fixture.Build<ComplexType>().OmitAutoProperties().Create();
            res.IntProperty.Should().Be(0);
            res.StringProperty.Should().BeNull();
            res.StringProperty2.Should().BeNull();
        }

        [Fact]
        public void CreatesComplexObjectUsingRules()
        {
            var fixture = new Fixture();
            fixture.Customize<ComplexType>(r => r.Without(c => c.StringProperty2).With(c => c.IntProperty, 100));
            var res = fixture.Create<ComplexType>();
            res.StringProperty2.Should().BeNull();
            res.IntProperty.Should().Be(100);
        }

        public class ComplexType
        {
            public string StringProperty { get; set; }
            public string StringProperty2 { get; set; }
            public int IntProperty { get; set; }
        }
    }
}
