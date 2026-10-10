using System;
using Xunit;

namespace Inno.Content.Tests;

public sealed class ContentKeyTests
{
    [Theory]
    [InlineData("/absolute")]
    [InlineData("a//b")]
    [InlineData("a/../b")]
    [InlineData("a/./b")]
    [InlineData("a\\b")]
    [InlineData("C:/root")]
    [InlineData("a ")]
    [InlineData("a.")]
    [InlineData("CON.txt")]
    [InlineData("nul/file")]
    [InlineData("a\u0000b")]
    public void NonportableKeysAreRejected(string value) => Assert.Throws<ArgumentException>(() => new ContentKey(value));

    [Fact]
    public void ExactCaseAndSourceNamesRemainSignificant()
    {
        Assert.NotEqual(new ContentKey("Assets/A.bin"), new ContentKey("Assets/a.bin"));
        Assert.Equal("plugin::~Samples".Replace("::", "-"), new ContentKey("plugin-~Samples").value);
    }
}
