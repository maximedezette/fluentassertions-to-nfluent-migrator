using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class HaveLengthTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldHaveLength()
    {
        const string fluentAssertions = "var.Should().HaveLength(0);";
        const string nfluentEquivalent = "Check.That(var).HasSize(0);";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}