using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class EmptyTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBeEmpty()
    {
        const string fluentAssertions = "var.Should().BeEmpty();";
        const string nfluentEquivalent = "Check.That(var).IsEmpty();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldNotBeEmpty()
    {
        const string fluentAssertions = "var.Should().NotBeEmpty();";
        const string nfluentEquivalent = "Check.That(var).Not.IsEmpty();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}