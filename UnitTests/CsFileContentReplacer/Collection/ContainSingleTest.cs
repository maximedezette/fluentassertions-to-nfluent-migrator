using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class ContainSingleTest: CsTestContentReplacerTest
{
    
    [Fact]
    public void Should_replace_ShouldContainSingle()
    {
        const string fluentAssertions = "var.Should().ContainSingle();";
        const string nfluentEquivalent = "Check.That(var).HasSize(1);";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldContainSingle_with_condition()
    {
        const string fluentAssertions = "var.Should().ContainSingle(x => x > 3);";
        const string nfluentEquivalent = "Check.That(var.Count(x => x > 3)).IsEqualTo(1);";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}