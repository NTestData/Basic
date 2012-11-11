using System;
using FluentAssertions;
using Xunit;

namespace NTestData.Basic.Tests
{
    public class TestDataTests
    {
        private class SampleClass
        {
            internal string Text { get; set; }

            internal long Ticks { get; private set; }

            internal static void Stamp(SampleClass obj)
            {
                obj.Ticks = DateTime.UtcNow.Ticks;
            }
        }

        private class SampleClassWithoutDefaultCtor
        {
            internal int Value { get; private set; }

            internal SampleClassWithoutDefaultCtor(int value)
            {
                Value = value;
            }

            internal static void SquareValue(SampleClassWithoutDefaultCtor obj)
            {
                obj.Value = obj.Value*obj.Value;
            }
        }

        [Fact]
        public void CanCreateSingleObject()
        {
            var result = TestData.Create<SampleClass>();

            result.Should().NotBeNull();
        }

        [Fact]
        public void CanCreateSingleObjectWithoutDefaultCtor()
        {
            const int sampleValue = 42;

            var result = TestData.Create(() => new SampleClassWithoutDefaultCtor(sampleValue));

            result.Should().NotBeNull();
            result.Value.Should().Be(sampleValue);
        }

        [Fact]
        public void CanCreateSingleCustomizedObject()
        {
            long ticksNow = DateTime.UtcNow.Ticks;
            const string stampText = "you're stamped";

            var obj = TestData.Create<SampleClass>(
                x => x.Text = stampText,
                SampleClass.Stamp);

            obj.Ticks.Should().BeGreaterOrEqualTo(ticksNow);
            obj.Text.Should().Be(stampText);
        }

        [Fact]
        public void CanCreateListOfObjects()
        {
            const ushort capacity = 11;

            var list = TestData.CreateListOf<SampleClass>(capacity);

            list.Should().HaveCount(capacity);
            foreach (var obj in list)
            {
                obj.Should().NotBeNull();
            }
        }

        [Fact]
        public void CanCreateListOfCustomizedObjects()
        {
            const ushort capacity = 3;
            long ticksNow = DateTime.UtcNow.Ticks;

            var list = TestData.CreateListOf<SampleClass>(capacity, SampleClass.Stamp);

            list.Should().HaveCount(capacity);
            foreach (var obj in list)
            {
                obj.Ticks.Should().BeGreaterOrEqualTo(ticksNow);
            }
        }

        [Fact]
        public void CanCreateListOfCustomizedObjectsWithoutDefaultCtor()
        {
            const ushort capacity = 7;
            ushort initialValue = 0;

            var list = TestData.CreateListOf(
                capacity,
                () => new SampleClassWithoutDefaultCtor(initialValue++),
                SampleClassWithoutDefaultCtor.SquareValue);

            list.Should().HaveCount(capacity);
            for (int index = 0; index < capacity; ++index)
            {
                list[index].Should().NotBeNull();
                list[index].Value.Should().Be(index*index);
            }
        }

        [Fact]
        public void CanCustomizeObjectUsingMultipleCustomizations()
        {
            var obj = new SampleClass();
            Action<SampleClass> greet = x => x.Text = "Hello, ";
            Action<SampleClass> world = x => x.Text += "World!";

            obj.Customize(greet, world);

            obj.Text.Should().Be("Hello, World!");
        }

        [Fact]
        public void CustomizeObjectReturnsSameObject()
        {
            var obj = new SampleClass();

            var resultObj = obj.Customize(x => x.Text = "foo bar");

            resultObj.Should().BeSameAs(obj);
        }

        [Fact]
        public void CustomizeObjectUsingNullFails()
        {
            var obj = new SampleClass();

            var exception = Record.Exception(
                () => obj.Customize(null));

            exception.Should().BeOfType<NullReferenceException>();
        }

        [Fact]
        public void CustomizeObjectWithOneOfTheCustomizationsBeingNullFails()
        {
            var obj = new SampleClass();
            var arrayOfCustomizationsContainingNull = new Action<SampleClass>[] {null};

            var exception = Record.Exception(
                () => obj.Customize(arrayOfCustomizationsContainingNull));

            exception.Should().BeOfType<NullReferenceException>();
        }
    }
}
