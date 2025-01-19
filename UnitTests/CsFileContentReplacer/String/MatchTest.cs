using NFluent;
using UnitTests;

public class MatchTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldMatch()
    {
        const string fluentAssertions = "var.Should().Match(\"regex\");";
        const string nfluentEquivalent = "Check.That(var).Matches(\"regex\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    

    [Fact]
    public void Should_replace_ShouldMatch_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".Single()" +
                                        ".Should().Match(\"regex\");";
        const string nfluentEquivalent = "Check.That(var.Single()).Matches(\"regex\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}