using fluentassertions_to_nfluent_migrator;
using NFluent;

namespace unit_tests;

public class ContainTest
{
    
    [Fact]
    public void Should_replace_ShouldContain()
    {
        const string fluentAssertions = "var.Should().Contain(\"item\");";
        const string nfluentEquivalent = "Check.That(var).Contains(\"item\");";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldNotContain()
    {
        const string fluentAssertions = "var.Should().NotContain(\"item\");";
        const string nfluentEquivalent = "Check.That(var).Not.Contains(\"item\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

}