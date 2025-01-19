using NFluent;
using UnitTests;

public class BeOfTypeTest : CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBeOfType()
    {
        const string fluentAssertions = "var.Should().BeOfType<string>();";
        const string nfluentEquivalent = "Check.That(var).IsInstanceOfType(typeof(string));";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    

    [Fact]
    public void Should_replace_ShouldBeOfType_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".Single()" +
                                        ".Should().BeOfType<string>();";
        const string nfluentEquivalent = "Check.That(var.Single()).IsInstanceOfType(typeof(string));";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}