using System.Text.RegularExpressions;

namespace CommandLine;

public class CsProjFileContentReplacer
{
    /// <summary>
    /// Replaces FluentAssertions package references with NFluent ones.
    /// </summary>
    /// <param name="content">The .csprojto process.</param>
    /// <returns>The updated .csproj code with replaced package references.</returns>
    public static string Replace(string content)
    {
        const string targetNfluentVersion = "3.1.0";
        var pattern = @"<\s*PackageReference\s+Include\s*=\s*""FluentAssertions""\s+Version\s*=\s*""[^""]+""\s*/>";

        var replacement = $@"<PackageReference Include=""NFluent"" Version=""{targetNfluentVersion}""/>";
        
        return Regex.Replace(content, pattern, replacement);
    }
}
