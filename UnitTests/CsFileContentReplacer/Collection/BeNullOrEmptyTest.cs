using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class BeNullOrEmptyTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_BeNullOrEmpty()
    {
        const string fluentAssertions = "var.List().Should().BeNullOrEmpty();";
        const string nfluentEquivalent = "Check.That(var.List()).IsNullOrEmpty();";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}