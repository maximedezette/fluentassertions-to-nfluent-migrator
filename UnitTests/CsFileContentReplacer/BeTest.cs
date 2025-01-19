using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class BeTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBe()
    {
        const string fluentAssertions = "var.Should().Be(42);";
        const string nfluentEquivalent = "Check.That(var).IsEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That<string>(actual).IsEqualTo(nfluentEquivalent);
    }
}