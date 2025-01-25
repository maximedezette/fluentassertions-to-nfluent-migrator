using NFluent;

namespace UnitTests.CsFileContentReplacer.Exception;

public class ThrowTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_Throw()
    {
        const string fluentAssertions = "var.Should().Throw<MyException>();";
        const string nfluentEquivalent = "Check.ThatCode(var).Throws<MyException>();";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_Throw_with_diamonds()
    {
        const string fluentAssertions = "action.Should().Throw<InvalidOperationException>();";
        const string nfluentEquivalent = "Check.ThatCode(action).Throws<InvalidOperationException>();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ThrowWithMessage()
    {
        const string fluentAssertions = "var.Should().Throw<MyException>().WithMessage(\"My message\");";
        const string nfluentEquivalent = "Check.ThatCode(var).Throws<MyException>().AndWhichMessage().Matches(\"My message\");";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}