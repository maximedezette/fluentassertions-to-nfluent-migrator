using NFluent;
using UnitTests;

public class StartWithTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldStartWith()
    {
        const string fluentAssertions = "var.Should().StartWith(\"prefix\");";
        const string nfluentEquivalent = "Check.That(var).StartsWith(\"prefix\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    

    [Fact]
    public void Should_replace_ShouldStartWith_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".Single()" +
                                        ".Should().StartWith(\"prefix\");";
        const string nfluentEquivalent = "Check.That(var.Single()).StartsWith(\"prefix\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}