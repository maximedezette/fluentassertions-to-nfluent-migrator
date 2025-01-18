using fluentassertions_to_nfluent_migrator;
using NFluent;

namespace unit_tests;

public class BeTest
{
    [Fact]
    public void Should_replace_ShouldBe()
    {
        const string fluentAssertions = "var.Should().Be(42);";
        const string nfluentEquivalent = "Check.That(var).IsEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}