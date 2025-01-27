using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public class WildcardsReplacer: Handler  
{
    // <summary>
    /// Replaces FluentAssertions wildcards with NFluent equivalent for assertions that support them
    /// </summary>
    /// <param name="content">The source code to process.</param>
    /// <returns>The updated source code with replaced assertions.</returns>
    public override string Handle(string content)
    {
        var regex = new Regex(@"\.Matches\((@?""[^""]*""|@?'[^']*')\)");

        return regex.Replace(content, match =>
        {
            var originalContent = match.Groups[1].Value; 
            var modifiedContent = originalContent.Replace("*", ".*");
            return $".Matches({modifiedContent})"; 
        });
    }
}