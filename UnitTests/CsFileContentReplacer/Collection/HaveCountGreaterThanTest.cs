using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class HaveCountGreaterThanTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_HaveCountGreaterThan()
    {
        const string fluentAssertions = "var.Should().HaveCountGreaterThan(3);";
        const string nfluentEquivalent = "Check.That(var).WhoseSize().IsStrictlyGreaterThan(3);";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }  
}