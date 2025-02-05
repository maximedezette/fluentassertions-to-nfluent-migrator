using NFluent;

namespace UnitTests.CsFileContentReplacer.Enum;

public class SameNameTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldHaveSameNameHas()
    {
        const string fluentAssertions = "MyEnum.One.Should().HaveSameNameAs(OtherEnum.One);";
        const string nfluentEquivalent = "Check.That(MyEnum.One.ToString()).IsEqualTo(OtherEnum.One.ToString());";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldNotHaveSameNameAs()
    {
        const string fluentAssertions = "MyEnum.One.Should().NotHaveSameNameAs(OtherEnum.One);";
        const string nfluentEquivalent = "Check.That(MyEnum.One.ToString()).IsNotEqualTo(OtherEnum.One.ToString());";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }   
}