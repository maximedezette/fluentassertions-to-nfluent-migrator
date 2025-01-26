using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public partial class CsFileContentReplacer
{
    /// <summary>
    /// Applies string-specific assertions replacement rules to the provided content.
    /// </summary>
    private static string ReplaceStringAssertions(string content)
    {
        var stringReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().BeNullOrWhiteSpace() -> Check.That(var).IsNullOrWhiteSpace();
            GetSubjectOnlyReplacement("BeNullOrWhiteSpace", "Check.That(${subject}).IsNullOrWhiteSpace();"),
           
            // .Should().EndWith(value) -> Check.That(var).EndsWith(value);
            GetSubjectValueReplacement("EndWith", "Check.That(${subject}).EndsWith(${value});"),
            
            // .Should().HaveLength(value) -> Check.That(var).HasSize(value);
            GetSubjectValueReplacement("HaveLength", "Check.That(${subject}).HasSize(${value});"),
            
            // .Should().NotBeNullOrWhiteSpace() -> Check.That(var).Not.IsNullOrWhiteSpace();
            GetSubjectOnlyReplacement("NotBeNullOrWhiteSpace", "Check.That(${subject}).Not.IsNullOrWhiteSpace();"),

            // .Should().StarsWith(value) -> Check.That(var).StartsWith(value);
            GetSubjectValueReplacement("StartWith", "Check.That(${subject}).StartsWith(${value});"),

            
        };

        foreach (var (pattern, replacement) in stringReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return content;
    }
}