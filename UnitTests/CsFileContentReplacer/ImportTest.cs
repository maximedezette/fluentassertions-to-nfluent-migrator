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
}