using NFluent;
using UnitTests;

public class BeGreaterOrEqualToTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBeGreaterOrEqualTo()
    {
        const string fluentAssertions = "var.Should().BeGreaterOrEqualTo(42);";
        const string nfluentEquivalent = "Check.That(var).IsGreaterOrEqualThan(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    

    [Fact]
    public void Should_replace_ShouldBeGreaterOrEqualTo_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".Single()" +
                                        ".Should().BeGreaterOrEqualTo(42);";
        const string nfluentEquivalent = "Check.That(var.Single()).IsGreaterOrEqualThan(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}