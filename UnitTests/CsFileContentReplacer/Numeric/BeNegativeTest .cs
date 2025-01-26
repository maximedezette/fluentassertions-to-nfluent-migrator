using NFluent;

namespace UnitTests.CsFileContentReplacer.Numeric;

public class BeNegativeTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBeNegative()
    {
        const string fluentAssertions = "var.Should().BeNegative();";
        const string nfluentEquivalent = "Check.That(var).IsStrictlyNegative();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    
}