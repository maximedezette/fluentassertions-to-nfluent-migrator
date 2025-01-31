using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public class NFluentMultipleAssertionReplacer: Handler  
{
    // <summary>
    /// Replace NFluent assertions with more specific ones when necessary
    /// </summary>
    /// <param name="content">The source code to process.</param>
    /// <returns>The updated source code with replaced assertions.</returns>
    public override string Handle(string content)
    {
        var replacements = new (string Pattern, string Replacement)[]
        {
            (@"Check\.That\((?<var>[^)]+)\)\.Contains\((?<predicate>[^\)]+=>[^\)]+)\)", "Check.That(${var}).HasElementThatMatches(${predicate})")
        };
        
        foreach (var (pattern, replacement) in replacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }
        
        return Next is not null ? Next.Handle(content) : content;
    }
}