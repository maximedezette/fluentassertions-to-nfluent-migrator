using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class ImportTest:CsTestContentReplacerTest
{
    [Theory]
    [InlineData("using FluentAssertions;")]
    [InlineData("using FluentAssertions.Common;")]
    public void Should_replace_Import(string fluentAssertionsImport)
    {
        const string nfluentEquivalent = "using NFluent;";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertionsImport);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_add_ApiCheck_Import_when_necessary()
    {
        const string fluentAssertions = "using FluentAssertions;\n" +
                                        "var.Should().Throw<MyException>().WithMessage(\"My message\");";
        const string nfluentEquivalent = "using NFluent;\n" +
                                         "using NFluent.ApiChecks;\n" +
                                         "" +
                                         "Check.ThatCode(var).Throws<MyException>().AndWhichMessage().Matches(\"My message\");";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}