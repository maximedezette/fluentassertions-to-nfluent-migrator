using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class ContainKeyTest: CsTestContentReplacerTest
{
    
    [Fact]
    public void Should_replace_ShouldContainKey()
    {
        const string fluentAssertions = "options.ResultStatusCodes.Should().ContainKey(HealthStatus.Healthy);";
        const string nfluentEquivalent = "Check.That(options.ResultStatusCodes.Keys).Contains(HealthStatus.Healthy);";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}