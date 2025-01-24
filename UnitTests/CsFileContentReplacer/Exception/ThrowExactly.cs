using NFluent;

namespace UnitTests.CsFileContentReplacer.Exception;

public class ThrowExactlyTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_Replace_ThrowExactly()
    {
        const string fluentAssertions = "action.Should().ThrowExactly<InvalidOperationException>();";
        const string nfluentEquivalent = "Check.ThatCode(action).Throws<InvalidOperationException>();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_Replace_ThrowExactlyWithMessage()
    {
        const string fluentAssertions = "action.Should().ThrowExactly<InvalidOperationException>().WithMessage(\"My message\");";
        const string nfluentEquivalent = "Check.ThatCode(action).Throws<InvalidOperationException>().WithMessage(\"My message\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

}