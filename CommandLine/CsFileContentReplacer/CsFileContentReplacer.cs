using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public partial class CsFileContentReplacer : IReplacer
{
    /// <summary>
    /// Replaces FluentAssertions syntax with NFluent syntax in the provided code string.
    /// </summary>
    /// <param name="content">The source code to process.</param>
    /// <returns>The updated source code with replaced assertions.</returns>
    public string Replace(string content)
    {
        content = Regex.Replace(content, @"using\s+FluentAssertions(\.\w+)*\s*;", "using NFluent;");

        content = ReplaceBooleanAssertions(content);
        
        content = ReplaceStringAssertions(content);
        
        content = ReplaceNumericAssertions(content);

        content = ReplaceExceptionAssertions(content);
        
        content = ReplaceCollectionAssertions(content);

        content = ReplaceWildcards(content);

        // Define other general replacements
        var generalReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().NotBeNull() -> Check.That(var).IsNotNull();
            GetSubjectOnlyReplacement("NotBeNull", "Check.That(${subject}).IsNotNull();"),

            // .Should().BeNull() -> Check.That(var).IsNull();
            GetSubjectOnlyReplacement("BeNull", "Check.That(${subject}).IsNull();"),
            
            // .Should().Be(value) -> Check.That(var).IsEqualTo(value);
            GetSubjectValueReplacement("Be", "Check.That(${subject}).IsEqualTo(${value});"),

            // .Should().NotBe(value) -> Check.That(var).IsNotEqualTo(value);
            GetSubjectValueReplacement("NotBe", "Check.That(${subject}).IsNotEqualTo(${value});"),

            // .Should().BeOfType(value) -> Check.That(var).IsInstanceOfType(value);
            GetSubjectValueReplacement("BeOfType", "Check.That(${subject}).IsInstanceOfType(${value});"),

            // .Should().BeOfType<value>() -> Check.That(var).IsInstanceOfType(value);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\s*\(\s*\)\s*\.BeOfType\s*<\s*(?<value>[^\>]+)\s*>\s*\(\s*\)\s*;",
                "Check.That(${subject}).IsInstanceOfType(typeof(${value}));"),

            // .Should().BeEquivalentTo(object) -> Check.That(var).HasFieldsWithSameValues(object);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\s*\(\s*\)\s*\.BeEquivalentTo\s*\(\s*(?<value>(?:(?>[^(){}]+|\((?<Open>)|(?<-Open>\))|\{(?<Open>)|(?<-Open>\}))*(?(Open)(?!)))+?)\s*\)\s*(?:,\s*""[^""]*""\s*)?;",
                "Check.That(${subject}).HasFieldsWithSameValues(${value});"),
            

            // .Should().Equal(value) -> Check.That(var).IsEqualTo(value);
            GetSubjectValueReplacement("Equal", "Check.That(${subject}).IsEqualTo(${value});"),

            // .Should().NotEqual(value) -> Check.That(var).IsNotEqualTo(value);
            GetSubjectValueReplacement("NotEqual", "Check.That(${subject}).IsNotEqualTo(${value});"),

            // .Execute.Assertion.FailWith(reason) -> Check.Fail(reason);
            (@"Execute\.Assertion\.FailWith\((?<message>\$""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*')\);",
                "Check.WithCustomMessage(${message}).That(false ).IsTrue();")
        };

        // Apply general replacements
        foreach (var (pattern, replacement) in generalReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return content;
    }

    private static (string, string) GetSubjectValueReplacement(string Pattern, string Replacement)
    {
        return (
            $@"(?<subject>\S(?:.*?\S)?)\s*\.Should\(\)\s*\.{Pattern}\s*\(\s*(?<value>(""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*'|\((?:[^()]*|\((?:[^()]*|\([^()]*\))*\))*\)|[^,()])+?)\s*(?:,\s*""[^""]*"")?\)\s*;",
            Replacement);
    }


    private static (string, string) GetSubjectOnlyReplacement(string Pattern, string Replacement)
    {
        return ($@"(?<subject>\S(?:.*\S)?)\s*\.Should\s*\(\s*\)\s*\.{Pattern}\s*\(\s*\)\s*(?:,\s*""[^""]*""\s*)?;",
            Replacement);
    }
}