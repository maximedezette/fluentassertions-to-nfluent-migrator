using NFluent;
using UnitTests;

public class BeLessOrEqualToTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBeLessOrEqualTo()
    {
        const string fluentAssertions = "var.Should().BeLessOrEqualTo(42);";
        const string nfluentEquivalent = "Check.That(var).IsLessOrEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    

    [Fact]
    public void Should_replace_ShouldBeLessOrEqualTo_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".Single()" +
                                        ".Should().BeLessOrEqualTo(42);";
        const string nfluentEquivalent = "Check.That(var.Single()).IsLessOrEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}