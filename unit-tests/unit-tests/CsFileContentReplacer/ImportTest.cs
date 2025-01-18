using fluentassertions_to_nfluent_migrator;
using NFluent;

namespace unit_tests;

public class ImportTest
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