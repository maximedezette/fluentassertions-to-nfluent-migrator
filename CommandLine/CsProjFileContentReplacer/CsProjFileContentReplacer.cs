using System.Text.RegularExpressions;

namespace CommandLine.CsProjFileContentReplacer;

public class CsProjFileContentReplacer : IReplacer
{
    /// <summary>
    /// Replaces FluentAssertions package references with NFluent ones.
    /// </summary>
    /// <param name="content">The .csprojto process.</param>
    /// <returns>The updated .csproj code with replaced package references.</returns>
    public string Replace(string content)
    {
        const string targetNfluentVersion = "3.1.0";
        var versionReplacements = new (string Pattern, string Replacement)[]
        {
            (@"<\s*PackageReference\s+Include\s*=\s*""FluentAssertions""\s+Version\s*=\s*""[^""]+""\s*/>", $@"<PackageReference Include=""NFluent"" Version=""{targetNfluentVersion}""/>"),
            (@"<\s*PackageReference\s+Include\s*=\s*""FluentAssertions""\s+/>", @"<PackageReference Include=""NFluent""/>"),
        };
        
        foreach (var (pattern, replacement) in versionReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return content;
    }
}
