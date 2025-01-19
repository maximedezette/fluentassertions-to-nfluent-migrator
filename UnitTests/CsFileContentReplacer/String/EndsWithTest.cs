using NFluent;
using UnitTests;

public class EndWithTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldEndWith()
    {
        const string fluentAssertions = "var.Should().EndWith(\"suffix\");";
        const string nfluentEquivalent = "Check.That(var).EndsWith(\"suffix\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    

    [Fact]
    public void Should_replace_ShouldEndWith_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".Single()" +
                                        ".Should().EndWith(\"suffix\");";
        const string nfluentEquivalent = "Check.That(var.Single()).EndsWith(\"suffix\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}