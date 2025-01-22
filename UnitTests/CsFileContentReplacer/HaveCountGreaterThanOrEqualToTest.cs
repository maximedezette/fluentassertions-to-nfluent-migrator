using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class HaveCountGreaterThanOrEqualToTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_HaveCountGreaterThanOrEqualTo()
    {
        const string fluentAssertions = "var.Should().HaveCountGreaterThanOrEqualTo(3);";
        const string nfluentEquivalent = "Check.That(var).WhoseSize().IsGreaterOrEqualThan(3);";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }  
}