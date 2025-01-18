﻿using CommandLine;
using NFluent;

namespace UnitTests;

public class CsprojTest
{
    
    [Theory]
    [InlineData("<PackageReference Include=\"FluentAssertions\" Version=\"6.12.0\"/>")]
    [InlineData("<PackageReference   Include=\"FluentAssertions\"   Version=\"7.0.0\"  />")]
    public void Should_replace_PackageReferences(string fluentAssertionsPackageName)
    {
        const string nfluentEquivalent = "<PackageReference Include=\"NFluent\" Version=\"3.1.0\"/>";
        
        var actual = CsProjFileContentReplacer.Replace(fluentAssertionsPackageName);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}