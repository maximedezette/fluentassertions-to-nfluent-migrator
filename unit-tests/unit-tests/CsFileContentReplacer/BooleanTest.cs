using fluentassertions_to_nfluent_migrator;
using NFluent;

namespace unit_tests;

public class BooleanTest
{
    [Fact]
    public void Should_replace_ShouldBeTrue()
    {
        const string fluentAssertions = "var.Should().BeTrue();";
        const string nfluentEquivalent = "Check.That(var).IsTrue();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldBeFalse()
    {
        const string fluentAssertions = "var.Should().BeFalse();";
        const string nfluentEquivalent = "Check.That(var).IsFalse();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldNotBeFalse()
    {
        const string fluentAssertions = "var.Should().NotBeFalse();";
        const string nfluentEquivalent = "Check.That(var).Not.IsFalse();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldNotBeTrue()
    {
        const string fluentAssertions = "var.Should().NotBeTrue();";
        const string nfluentEquivalent = "Check.That(var).Not.IsTrue();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldBeTrueForNullableBoolean()
    {
        const string fluentAssertions = "theBoolean.Should().BeTrue();";
        const string nfluentEquivalent = "Check.That(theBoolean).IsTrue();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldBeFalseForNullableBoolean()
    {
        const string fluentAssertions = "theBoolean.Should().BeFalse();";
        const string nfluentEquivalent = "Check.That(theBoolean).IsFalse();";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ImplyAssertion()
    {
        const string fluentAssertions = "theBoolean.Should().Imply(anotherBoolean);";
        const string nfluentEquivalent = "Check.That(theBoolean).Imply(anotherBoolean);";
        
        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}