using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class BeTest: CsTestContentReplacerTest
{
    [Fact]
    public void Should_replace_ShouldBe()
    {
        const string fluentAssertions = "var.Should().Be(42);";
        const string nfluentEquivalent = "Check.That(var).IsEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    
    
    [Fact]
    public void Should_replace_ShouldBe_with_failure_log()
    {
        const string fluentAssertions = "var.Should().Be(42, \"because they have the same values\");";
        const string nfluentEquivalent = "Check.That(var).IsEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldBe_with_multi_lines()
    {
        const string fluentAssertions = "var" +
                                        ".Single()" +
                                        ".Should().Be(42, \"because they have the same values\");";
        const string nfluentEquivalent = "Check.That(var.Single()).IsEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldBe_with_complex_subject()
    {
        const string fluentAssertions = "object!.MyMethod(arg1, arg2).Should().Be(42, \"because they have the same values\");";
        const string nfluentEquivalent = "Check.That(object!.MyMethod(arg1, arg2)).IsEqualTo(42);";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
    
    [Fact]
    public void Should_replace_ShouldBe_with_complex_value()
    {
        const string fluentAssertions = "var.Should().Be(await SomeMethod(arg1 , arg2), \"because they have the same values\");";
        const string nfluentEquivalent = "Check.That(var).IsEqualTo(await SomeMethod(arg1 , arg2));";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }    
    
    [Fact]
    public void Should_replace_ShouldBe_with_complex_value_2()
    {
        const string fluentAssertions = "response.Headers[HttpResponseHeader.Expires].Should().Be(\"Thu, 01 Jan 1970 00:00:00 GMT\");";
        const string nfluentEquivalent = "Check.That(response.Headers[HttpResponseHeader.Expires]).IsEqualTo(\"Thu, 01 Jan 1970 00:00:00 GMT\");";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);

        Check.That(actual).IsEqualTo(nfluentEquivalent);
    }
}