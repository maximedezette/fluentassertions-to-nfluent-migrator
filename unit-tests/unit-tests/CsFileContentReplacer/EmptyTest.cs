using fluentassertions_to_nfluent_migrator;
using NFluent;

namespace unit_tests;

public class EmptyTest
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
        const string nfluentEquivalent = "Check.That(var).IsNotEmpty();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}