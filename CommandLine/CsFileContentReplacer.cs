using System.Text.RegularExpressions;

namespace CommandLine;

public class CsFileContentReplacer
{
    /// <summary>
    /// Replaces FluentAssertions syntax with NFluent syntax in the provided code string.
    /// </summary>
    /// <param name="content">The source code to process.</param>
    /// <returns>The updated source code with replaced assertions.</returns>
    public static string Replace(string content)
    {
        content = Regex.Replace(content, @"using\s+FluentAssertions(\.\w+)*\s*;", "using NFluent;");

        // Apply boolean-specific replacements
        content = ReplaceBooleanAssertions(content);

        // Apply nullable-specific replacements
        content = ReplaceNullableAssertions(content);

        // Apply exception-specific replacements
        content = ReplaceExceptionAssertions(content);

        // Define other general replacements
        var generalReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().NotBeNull() -> Check.That(var).IsNotNull();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.NotBeNull\s*\(\s*\)\s*;", "Check.That(${subject}).IsNotNull();"),
                
            // .Should().BeNull() -> Check.That(var).IsNull();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.BeNull\s*\(\s*\)\s*;", "Check.That(${subject}).IsNull();"),
                
            // .Should().Be(value) -> Check.That(var).IsEqualTo(value);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.Be\s*\(\s*(?<value>.+?)\s*\)\s*;", "Check.That(${subject}).IsEqualTo(${value});"),
            
            // .Should().BeEquivalentTo(object) -> Check.That(var).IsEquivalentTo(object);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.BeEquivalentTo\s*\(\s*(?<object>.+?)\s*\)\s*;", "Check.That(${subject}).HasFieldsWithSameValues(${object});"),
            
            // .Should().Contain(value) -> Check.That(var).Contains(value);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.Contain\s*\(\s*(?<value>.+?)\s*\)\s*;", "Check.That(${subject}).Contains(${value});"),
                
            // .Should().NotContain(value) -> Check.That(var).Not.Contains(value);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.NotContain\s*\(\s*(?<value>.+?)\s*\)\s*;", "Check.That(${subject}).Not.Contains(${value});"),
            
            // .Should().BeEmpty() -> Check.That(var).IsEmpty();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.BeEmpty\s*\(\s*\)\s*;", "Check.That(${subject}).IsEmpty();"),
                
            // .Should().NotBeEmpty() -> Check.That(var).IsNotEmpty();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.NotBeEmpty\s*\(\s*\)\s*;", "Check.That(${subject}).IsNotEmpty();")
        };

        // Apply general replacements
        foreach (var (pattern, replacement) in generalReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return content;
    }

    /// <summary>
    /// Applies exception-specific assertions replacement rules to the provided content.
    /// </summary>
    private static string ReplaceExceptionAssertions(string content)
    {
        var exceptionReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().Throw<ExceptionType>() -> Check.ThatCode(action).Throws<ExceptionType>();
            (@"(?<action>\S(?:.*\S)?)\s*\.Should\(\)\s*\.Throw\s*<(?<exceptionType>[^>]+)>\s*\(\s*\)\s*;", 
                "Check.ThatCode(${action}).Throws<${exceptionType}>();"),
        
            // .Should().ThrowExactly<ExceptionType>() -> Check.ThatCode(action).ThrowsExactly<ExceptionType>();
            (@"(?<action>\S(?:.*\S)?)\s*\.Should\(\)\s*\.ThrowExactly\s*<(?<exceptionType>[^>]+)>\s*\(\s*\)\s*;", 
                "Check.ThatCode(${action}).ThrowsExactly<${exceptionType}>();"),
        
            // .Should().NotThrow() -> Check.ThatCode(action).DoesNotThrow();
            (@"(?<action>\S(?:.*\S)?)\s*\.Should\(\)\s*\.NotThrow\s*\(\s*\)\s*;", 
                "Check.ThatCode(${action}).DoesNotThrow();")
        };

        // Apply exception-specific replacements
        foreach (var (pattern, replacement) in exceptionReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return content;
    }

    /// <summary>
    /// Applies boolean-specific assertions replacement rules to the provided content.
    /// </summary>
    private static string ReplaceBooleanAssertions(string content)
    {
        var booleanReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().BeTrue() -> Check.That(var).IsTrue();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.BeTrue\s*\(\s*\)\s*;", "Check.That(${subject}).IsTrue();"),
                
            // .Should().BeFalse() -> Check.That(var).IsFalse();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.BeFalse\s*\(\s*\)\s*;", "Check.That(${subject}).IsFalse();"),
            
            // .Should().NotBeFalse() -> Check.That(var).Not.IsFalse();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.NotBeFalse\s*\(\s*\)\s*;", "Check.That(${subject}).Not.IsFalse();"),

            // .Should().NotBeTrue() -> Check.That(var).Not.IsTrue();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.NotBeTrue\s*\(\s*\)\s*;", "Check.That(${subject}).Not.IsTrue();"),

            // .Should().Imply(other) -> Check.That(var).Imply(other);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.Imply\s*\(\s*(?<other>\S(?:.*\S)?)\s*\)\s*;", "Check.That(${subject}).Imply(${other});")
        };

        // Apply boolean-specific replacements
        foreach (var (pattern, replacement) in booleanReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return content;
    }

    /// <summary>
    /// Applies nullable-specific assertions replacement rules to the provided content.
    /// </summary>
    private static string ReplaceNullableAssertions(string content)
    {
        var nullableReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().NotHaveValue() -> Check.That(var).Not.HasValue();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.NotHaveValue\s*\(\s*\)\s*;", "Check.That(${subject}).Not.HasValue();"),

            // .Should().HaveValue() -> Check.That(var).HasValue();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.HaveValue\s*\(\s*\)\s*;", "Check.That(${subject}).HasValue();"),

            // .Should().Match(x => !x.HasValue || x > 0) -> Check.That(var).Matches(x => !x.HasValue || x > 0);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.Match\s*\(\s*(?<lambda>.+?)\s*\)\s*;", "Check.That(${subject}).Matches(${lambda});")
        };

        // Apply nullable-specific replacements
        foreach (var (pattern, replacement) in nullableReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return content;
    }
}
