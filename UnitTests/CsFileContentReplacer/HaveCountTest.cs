using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class HaveCountTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldHaveCount()
    {
        const string fluentAssertions = "var.Should().HaveCount(3);";
        const string nfluentEquivalent = "Check.That(var).HasSize(3);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }  
    
    [Fact]
    public void Should_replace_ShouldHaveCount_with_failure_log()
    {
        const string fluentAssertions = "var.Should().HaveCount(3, \"because we expect 3 elements\");";
        const string nfluentEquivalent = "Check.That(var).HasSize(3);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldHaveCount_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".List()" +
                                        ".Should().HaveCount(3, \"because we expect 3 elements\");";
        const string nfluentEquivalent = "Check.That(var.List()).HasSize(3);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}