using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class BeGreaterThanTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBeGreaterThan()
    {
        const string fluentAssertions = "var.Should().BeGreaterThan(42);";
        const string nfluentEquivalent = "Check.That(var).IsGreaterThan(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    

    [Fact]
    public void Should_replace_ShouldBeGreaterThan_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".Single()" +
                                        ".Should().BeGreaterThan(42);";
        const string nfluentEquivalent = "Check.That(var.Single()).IsGreaterThan(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}