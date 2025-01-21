using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class BeNullOrWhiteSpaceTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBeNullOrWhiteSpaceTest()
    {
        const string fluentAssertions = "var.Should().BeNullOrWhiteSpace();";
        const string nfluentEquivalent = "Check.That(var).IsNullOrWhiteSpace();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}