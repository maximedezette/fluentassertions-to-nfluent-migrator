using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class NotBeTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldNotBe()
    {
        const string fluentAssertions = "var.Should().NotBe(42);";
        const string nfluentEquivalent = "Check.That(var).IsNotEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldNotBeWithFailureLog()
    {
        const string fluentAssertions = "var.Should().NotBe(42, \"because they have the same values\");";
        const string nfluentEquivalent = "Check.That(var).IsNotEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}