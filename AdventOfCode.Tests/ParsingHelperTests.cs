using AdventOfCode2024.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode.Tests;

[TestFixture]
public class ParsingHelperTests
{
    [SetUp]
    public void SetUp()
    {
    }

    [Test]
    public void ToArray2D()
    {
        // Arrange
        var input = "adadad2iad68";
        var delimeters = new char[] { 'a', 'd' };
        var test = new char[][] { [], [], [], ['2', 'i'], ['6', '8'] };

        // Act
        var result = ParsingHelper.ToArray2D(input, delimeters);

        // Assert
        result.Should().BeEquivalentTo(test);
    }
}
