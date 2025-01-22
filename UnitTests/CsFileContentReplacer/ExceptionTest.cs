using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class ExceptionTest: CsTestContentReplacerTest
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

    [Fact]
    public void Should_Replace_NotThrow()
    {
        const string fluentAssertions = "action.Should().NotThrow();";
        const string nfluentEquivalent = "Check.ThatCode(action).DoesNotThrow();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
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
    
    [Fact]
    public void Should_replace_Throw()
    {
        const string fluentAssertions = "var.Should().Throw<MyException>();";
        const string nfluentEquivalent = "Check.ThatCode(var).Throws<MyException>();";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ThrowWithMessage()
    {
        const string fluentAssertions = "var.Should().Throw<MyException>().WithMessage(\"My message\");";
        const string nfluentEquivalent = "Check.ThatCode(var).Throws<MyException>().WithMessage(\"My message\");";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}