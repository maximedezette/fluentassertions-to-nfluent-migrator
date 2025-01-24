using NFluent;

namespace UnitTests.CsFileContentReplacer.Exception;

public class NotThrowTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_Replace_NotThrow()
    {
        const string fluentAssertions = "action.Should().NotThrow();";
        const string nfluentEquivalent = "Check.ThatCode(action).DoesNotThrow();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}