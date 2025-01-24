using NFluent;

namespace UnitTests;

public class ExecuteAssertionFailWithTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ExecuteAssertionFailWithTest()
    {
        const string fluentAssertions = "Execute.Assertion.FailWith($\"Fail because: {object.Value}\");";
        const string nfluentEquivalent = "Check.WithCustomMessage($\"Fail because: {object.Value}\").That(false ).IsTrue();";

        string actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}