using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class BeLessThanTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBeLessThan()
    {
        const string fluentAssertions = "var.Should().BeLessThan(42);";
        const string nfluentEquivalent = "Check.That(var).IsLessThan(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    

    [Fact]
    public void Should_replace_ShouldBeLessThan_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".Single()" +
                                        ".Should().BeLessThan(42);";
        const string nfluentEquivalent = "Check.That(var.Single()).IsLessThan(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}