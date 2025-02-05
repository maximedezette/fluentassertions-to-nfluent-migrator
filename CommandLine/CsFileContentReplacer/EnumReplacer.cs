using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public class EnumReplacer : Handler{
    /// <summary>
    /// Applies enumeration-specific assertions replacement rules to the provided content.
    /// </summary>
    public override string Handle(string content)
    {
        var enumReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().HaveFlag(value) -> Check.That(var).HasFlag(value);
            GetSubjectValueReplacement("HaveFlag", "Check.That(${subject}).HasFlag(${value});"),
            
            // .Should().NotHaveFlag(value) -> Check.That(var).Not.HasFlag(value);
            GetSubjectValueReplacement("NotHaveFlag", "Check.That(${subject}).Not.HasFlag(${value});"),
        };

        foreach (var (pattern, replacement) in enumReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return Next is not null ? Next.Handle(content) : content;
    }
}