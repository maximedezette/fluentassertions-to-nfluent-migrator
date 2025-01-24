using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class HaveSameCountTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldHaveSameCount()
    {
        const string fluentAssertions = "var.Should().HaveSameCount(otherVar);";
        const string nfluentEquivalent = "Check.That(var).HasSameSizeAs(otherVar);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }  
    
    [Fact]
    public void Should_replace_ShouldHaveSameCount_with_failure_log()
    {
        const string fluentAssertions = "var.Should().HaveSameCount(otherVar, \"because they should have the same number of elements\");";
        const string nfluentEquivalent = "Check.That(var).HasSameSizeAs(otherVar);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldHaveSameCount_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".List()" +
                                        ".Should().HaveSameCount(otherVar, \"because they should have the same number of elements\");";
        const string nfluentEquivalent = "Check.That(var.List()).HasSameSizeAs(otherVar);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}