using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class BeTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBe()
    {
        const string fluentAssertions = "var.Should().Be(42);";
        const string nfluentEquivalent = "Check.That(var).IsEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    
    
    [Fact]
    public void Should_replace_ShouldBe_with_failure_log()
    {
        const string fluentAssertions = "var.Should().Be(42, \"because they have the same values\");";
        const string nfluentEquivalent = "Check.That(var).IsEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldBe_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".Single()" +
                                        ".Should().Be(42, \"because they have the same values\");";
        const string nfluentEquivalent = "Check.That(var.Single()).IsEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}