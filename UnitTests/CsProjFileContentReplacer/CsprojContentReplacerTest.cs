using NFluent;

namespace UnitTests.CsProjFileContentReplacer;

public class CsprojContentReplacerTest: CsProjContentReplacerTest
{
    
    [Theory]
    [InlineData("<PackageReference Include=\"FluentAssertions\" Version=\"6.12.0\"/>")]
    [InlineData("<PackageReference   Include=\"FluentAssertions\"   Version=\"7.0.0\"  />")]
    public void Should_replace_PackageReferences_with_version(string fluentAssertionsPackageName)
    {
        const string nfluentEquivalent = "<PackageReference Include=\"NFluent\" Version=\"3.1.0\"/>";
        
        var actual = CsProjFileContentReplacer.Replace(fluentAssertionsPackageName);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Theory]
    [InlineData("<PackageReference Include=\"FluentAssertions\" />")]
    [InlineData("< PackageReference   Include=\"FluentAssertions\"    />")]
    public void Should_replace_PackageReferences_without_version(string fluentAssertionsPackageName)
    {
        const string nfluentEquivalent = "<PackageReference Include=\"NFluent\"/>";
        
        var actual = CsProjFileContentReplacer.Replace(fluentAssertionsPackageName);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}