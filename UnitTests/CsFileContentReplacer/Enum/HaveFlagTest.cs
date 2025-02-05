using NFluent;

namespace UnitTests.CsFileContentReplacer.Enum;

public class HaveFlagTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldHaveFlag()
    {
        const string fluentAssertions = "var.Should().HaveFlag(myFlag);";
        const string nfluentEquivalent = "Check.That(var).HasFlag(myFlag);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }   
    
    [Fact]
    public void Should_replace_ShouldNotHaveFlag()
    {
        const string fluentAssertions = "var.Should().NotHaveFlag(myFlag);";
        const string nfluentEquivalent = "Check.That(var).Not.HasFlag(myFlag);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }   
    
}