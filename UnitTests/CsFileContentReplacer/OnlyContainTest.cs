using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class OnlyContainTest: CsTestContentReplacerTest
{
    
    [Fact]
    public void Should_replace_ShouldOnlyContain()
    {
        const string fluentAssertions = "var.Should().OnlyContain(i => i <= 10);";
        const string nfluentEquivalent = "Check.That(var).ContainsOnlyElementsThatMatch(i => i <= 10);";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    
    
    [Fact]
    public void Should_replace_ShouldOnlyContain_with_complex_example()
    {
        const string fluentAssertions = "object.someAttribute?.someReferences" +
                                        "   .Should()" +
                                        "   .OnlyContain(i => i <= 10 " +
                                        "   || i == 15);";
        const string nfluentEquivalent = "Check.That(object.someAttribute?.someReferences).ContainsOnlyElementsThatMatch(i => i <= 10    || i == 15);";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
}