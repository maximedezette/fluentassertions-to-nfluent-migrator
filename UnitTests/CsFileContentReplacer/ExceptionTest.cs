using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class ExceptionTest: CsTestContentReplacerTest
{
    
    [Fact]
    public void Should_replace_NotThrowAsync()
    {
        const string fluentAssertions = "var.Should().NotThrowAsync();";
        const string nfluentEquivalent = "Check.ThatCode(var).DoesNotThrow();";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_NotThrowAsyncWithAwait()
    {
        const string fluentAssertions = "await var.Should().NotThrowAsync();";
        const string nfluentEquivalent = "Check.ThatCode(var).DoesNotThrow();";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}