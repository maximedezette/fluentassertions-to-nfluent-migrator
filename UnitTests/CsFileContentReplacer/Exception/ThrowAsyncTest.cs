using NFluent;

namespace UnitTests.CsFileContentReplacer.Exception;

public class ThrowAsyncTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_Replace_ThrowAsync()
    {
        const string fluentAssertions = "await act.Should().ThrowAsync<InternalErrorException>();";
        const string nfluentEquivalent = "Check.ThatCode(() => act()).ThrowsType(typeof(InternalErrorException));";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_Replace_ThrowAsyncWithMessage()
    {
        const string fluentAssertions = "await act.Should()" +
                                        "\n.ThrowAsync<InternalErrorException>()" +
                                        ".WithMessage(\"Failed\");";
        const string nfluentEquivalent = "Check.ThatCode(() => act())" +
                                         ".ThrowsType(typeof(InternalErrorException))" +
                                         ".AndWhichMessage()" +
                                         ".Matches(\"Failed\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_Replace_ThrowAsyncWithMessage_including_wildcard()
    {
        const string fluentAssertions = "await act.Should()" +
                                        "\n.ThrowAsync<InternalErrorException>()" +
                                        ".WithMessage(\"Failed because '*'\");";
        const string nfluentEquivalent = "Check.ThatCode(() => act())" +
                                         ".ThrowsType(typeof(InternalErrorException))" +
                                         ".AndWhichMessage()" +
                                         ".Matches(\"Failed because '.*'\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

}