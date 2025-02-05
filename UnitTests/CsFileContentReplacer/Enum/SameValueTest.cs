using NFluent;

namespace UnitTests.CsFileContentReplacer.Enum;

public class SameValueTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldHaveSameValueAs()
    {
        const string fluentAssertions = "MyEnum.One.Should().HaveSameValueAs(OtherEnum.One);";
        const string nfluentEquivalent = "Check.That(MyEnum.One).Considering().Fields.IsEqualTo(OtherEnum.One);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldNotHaveSamValueAs()
    {
        const string fluentAssertions = "MyEnum.One.Should().NotHaveSameValueAs(OtherEnum.One);";
        const string nfluentEquivalent = "Check.That(MyEnum.One).Considering().Fields.IsNotEqualTo(OtherEnum.One);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }   
}