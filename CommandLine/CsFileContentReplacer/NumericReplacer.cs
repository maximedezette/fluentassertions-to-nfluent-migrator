using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public class NumericReplacer : Handler{
    /// <summary>
    /// Applies numeric-specific assertions replacement rules to the provided content.
    /// </summary>
    public override string Handle(string content)
    {
        var numericReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().BeGreaterThan(value) -> Check.That(var).IsGreaterThan(value);
            GetSubjectValueReplacement("BeGreaterThan", "Check.That(${subject}).IsGreaterThan(${value});"),

            // .Should().BeGreaterOrEqualTo(value) -> Check.That(var).IsGreaterOrEqualTo(value);
            GetSubjectValueReplacement("BeGreaterOrEqualTo", "Check.That(${subject}).IsGreaterOrEqualThan(${value});"),

            // .Should().BeLessThan(value) -> Check.That(var).IsLessThan(value);
            GetSubjectValueReplacement("BeLessThan", "Check.That(${subject}).IsLessThan(${value});"),

            // .Should().BeLessOrEqualTo(value) -> Check.That(var).IsLessOrEqualTo(value);
            GetSubjectValueReplacement("BeLessOrEqualTo", "Check.That(${subject}).IsLessOrEqualTo(${value});"),
            
            // .Should().BePositive() -> Check.That(var).IsStrictlyPositive();
            GetSubjectOnlyReplacement("BePositive", "Check.That(${subject}).IsStrictlyPositive();"),
            
            // .Should().BeNegative() -> Check.That(var).IsStrictlyNegative();
            GetSubjectOnlyReplacement("BeNegative", "Check.That(${subject}).IsStrictlyNegative();"),
        };

        foreach (var (pattern, replacement) in numericReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return Next is not null ? Next.Handle(content) : content;
    }
}