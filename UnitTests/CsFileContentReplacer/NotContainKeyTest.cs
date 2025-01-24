using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class NotContainKeyTest: CsTestContentReplacerTest
{
    
    [Fact]
    public void Should_replace_ShouldNotContainKey()
    {
        const string fluentAssertions = "options.ResultStatusCodes.Should().NotContainKey(HealthStatus.Healthy);";
        const string nfluentEquivalent = "Check.That(options.ResultStatusCodes.Keys).Not.Contains(HealthStatus.Healthy);";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}