﻿using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class BeEquivalentToTests: CsTestContentReplacerTest
{
    
    [Fact]
    public void Should_replace_BeEquivalentTo()
    {
        const string fluentAssertions = "var.Should().BeEquivalentTo(otherObject);";
        const string nfluentEquivalent = "Check.That(var).HasFieldsWithSameValues(otherObject);";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_BeEquivalentTo_with_spaces()
    {
        const string fluentAssertions = "var   .Should()   .BeEquivalentTo(    otherObject   )   ;";
        const string nfluentEquivalent = "Check.That(var).HasFieldsWithSameValues(otherObject   );";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_BeEquivalentTo_with_complex_objects()
    {
        const string fluentAssertions = "complexVar.Should().BeEquivalentTo(new { Id = 1, Name = \"Test\" });";
        const string nfluentEquivalent = "Check.That(complexVar).HasFieldsWithSameValues(new { Id = 1, Name = \"Test\" });";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_BeEquivalentTo_with_complex_objects_and_multiple_lines()
    {
        const string fluentAssertions = "complexVar.Should()" +
                                        "   .BeEquivalentTo(" +
                                        "       new[] " +
                                        "       {" +
                                        "           Id = 1," +
                                        "            Name = \"Test\" " +
                                        "       });";
        const string nfluentEquivalent = "Check.That(complexVar).HasFieldsWithSameValues(new[]        {           Id = 1,            Name = \"Test\"        });";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}