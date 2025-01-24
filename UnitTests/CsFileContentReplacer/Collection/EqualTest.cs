using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class EqualTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldEqual()
    {
        const string fluentAssertions = "var.Should().Equal(other);";
        const string nfluentEquivalent = "Check.That(var).IsEqualTo(other);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }  
}