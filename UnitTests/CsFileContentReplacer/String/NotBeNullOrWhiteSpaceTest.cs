using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class NotBeNullOrWhiteSpaceTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldNotBeNullOrWhiteSpaceTest()
    {
        const string fluentAssertions = "var.Should().NotBeNullOrWhiteSpace();";
        const string nfluentEquivalent = "Check.That(var).Not.IsNullOrWhiteSpace();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}