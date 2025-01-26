using NFluent;

namespace UnitTests.CsFileContentReplacer.Numeric;

public class BePositiveTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBePositive()
    {
        const string fluentAssertions = "var.Should().BePositive();";
        const string nfluentEquivalent = "Check.That(var).IsStrictlyPositive();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    
}