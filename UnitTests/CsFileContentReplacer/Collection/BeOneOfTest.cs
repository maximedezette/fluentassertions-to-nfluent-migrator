using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class BeOneOfTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_BeOneOf()
    {
        const string fluentAssertions = "var.Should().BeOneOf(collection);";
        const string nfluentEquivalent = "Check.That(collection).Contains(var);";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}