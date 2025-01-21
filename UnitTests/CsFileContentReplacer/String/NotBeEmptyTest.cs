using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class NotBeEmptyTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldNotBeEmpty()
    {
        const string fluentAssertions = "var.Should().NotBeEmpty();";
        const string nfluentEquivalent = "Check.That(var).Not.IsEmpty();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}