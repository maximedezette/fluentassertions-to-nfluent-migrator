using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class NotBeNullOrEmptyTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_NotBeNullOrEmpty()
    {
        const string fluentAssertions = "var.List().Should().NotBeNullOrEmpty();";
        const string nfluentEquivalent = "Check.That(var.List()).Not.IsNullOrEmpty();";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}