using CommandLine;
using NFluent;

namespace UnitTests;

public class ExceptionTest
{
    [Fact]
    public void Should_Replace_Throw()
    {
        const string fluentAssertions = "action.Should().Throw<InvalidOperationException>();";
        const string nfluentEquivalent = "Check.ThatCode(action).Throws<InvalidOperationException>();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_Replace_ThrowExactly()
    {
        const string fluentAssertions = "action.Should().ThrowExactly<InvalidOperationException>();";
        const string nfluentEquivalent = "Check.ThatCode(action).ThrowsExactly<InvalidOperationException>();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_Replace_NotThrow()
    {
        const string fluentAssertions = "action.Should().NotThrow();";
        const string nfluentEquivalent = "Check.ThatCode(action).DoesNotThrow();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}