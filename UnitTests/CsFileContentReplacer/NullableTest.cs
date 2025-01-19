using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class NullableTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldNotHaveValue_with_NullableTypes()
    {
        const string fluentAssertions = "theShort.Should().NotHaveValue();";
        const string nfluentEquivalent = "Check.That(theShort).Not.HasValue();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldHaveValue_with_NullableTypes()
    {
        const string fluentAssertions = "theInt.Should().HaveValue();";
        const string nfluentEquivalent = "Check.That(theInt).HasValue();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldNotBeNull()
    {
        const string fluentAssertions = "var.Should().NotBeNull();";
        const string nfluentEquivalent = "Check.That(var).IsNotNull();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldBeNull_with_NullableTypes()
    {
        const string fluentAssertions = "theDate.Should().BeNull();";
        const string nfluentEquivalent = "Check.That(theDate).IsNull();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }

    [Fact]
    public void Should_replace_ShouldMatch_with_NullableTypes()
    {
        const string fluentAssertions = "theShort.Should().Match(x => !x.HasValue || x > 0);";
        const string nfluentEquivalent = "Check.That(theShort).Matches(x => !x.HasValue || x > 0);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}