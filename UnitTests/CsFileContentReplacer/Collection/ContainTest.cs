using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class ContainTest: CsTestContentReplacerTest
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
    public void Should_replace_ShouldContain_with_predicate()
    {
        const string fluentAssertions = "var.Should().Contain(x => x.Length > 3);";
        const string nfluentEquivalent = "Check.That(var).HasElementThatMatches(x => x.Length > 3);";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
        
    [Fact]
    public void Should_replace_ShouldContain_with_complex_subject()
    {
        const string fluentAssertions = "object!.MyMethod(some args).Should().Contain(\"item\");";
        const string nfluentEquivalent = "Check.That(object!.MyMethod(some args)).Contains(\"item\");";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldContain_with_spaces()
    {
        const string fluentAssertions = "var.Should().Contain(\"other item with many spaces\");";
        const string nfluentEquivalent = "Check.That(var).Contains(\"other item with many spaces\");";
        
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