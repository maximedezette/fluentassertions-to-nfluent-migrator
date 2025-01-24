using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class NotEqualTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldNotEqual()
    {
        const string fluentAssertions = "var.Should().NotEqual(other);";
        const string nfluentEquivalent = "Check.That(var).IsNotEqualTo(other);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }  
}