using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public class ImportReplacer: Handler
{
    /// <summary>
    /// Applies import-specific assertions replacement rules to the provided content.
    /// </summary>
    public override string Handle(string content)
    {
        content = Regex.Replace(content, @"using\s+FluentAssertions(\.\w+)*\s*;", "using NFluent;");

        if (content.Contains("AndWhichMessage"))
        {
            content = content.Replace("using NFluent;", "using NFluent;\nusing NFluent.ApiChecks;");
        }
        
        return Next is not null ? Next.Handle(content) : content;
    }
}