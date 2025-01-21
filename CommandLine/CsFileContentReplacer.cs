using System.Text.RegularExpressions;

namespace CommandLine;

public class CsFileContentReplacer : IReplacer
{
    /// <summary>
    /// Replaces FluentAssertions syntax with NFluent syntax in the provided code string.
    /// </summary>
    /// <param name="content">The source code to process.</param>
    /// <returns>The updated source code with replaced assertions.</returns>
    public string Replace(string content)
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
            GetSubjectOnlyReplacement("NotBeNull", "Check.That(${subject}).IsNotNull();"),
                
            // .Should().BeNull() -> Check.That(var).IsNull();
            GetSubjectOnlyReplacement("BeNull", "Check.That(${subject}).IsNull();"),
            
            // .Should().BeNullOrWhiteSpace() -> Check.That(var).IsNullOrWhiteSpace();
            GetSubjectOnlyReplacement("BeNullOrWhiteSpace", "Check.That(${subject}).IsNullOrWhiteSpace();"),

                
            // .Should().Be(value) -> Check.That(var).IsEqualTo(value);
            GetSubjectValueReplacement("Be", "Check.That(${subject}).IsEqualTo(${value});"),
            
            // .Should().NotBe(value) -> Check.That(var).IsNotEqualTo(value);
            GetSubjectValueReplacement("NotBe", "Check.That(${subject}).IsNotEqualTo(${value});"),
            
            // .Should().BeGreaterThan(value) -> Check.That(var).IsGreaterThan(value);
            GetSubjectValueReplacement("BeGreaterThan", "Check.That(${subject}).IsGreaterThan(${value});"),
            
            // .Should().BeGreaterOrEqualTo(value) -> Check.That(var).IsGreaterOrEqualTo(value);
            GetSubjectValueReplacement("BeGreaterOrEqualTo", "Check.That(${subject}).IsGreaterOrEqualTo(${value});"),
            
            // .Should().BeLessThan(value) -> Check.That(var).IsLessThan(value);
            GetSubjectValueReplacement("BeLessThan", "Check.That(${subject}).IsLessThan(${value});"),
            
            // .Should().BeLessOrEqualTo(value) -> Check.That(var).IsLessOrEqualTo(value);
            GetSubjectValueReplacement("BeLessOrEqualTo", "Check.That(${subject}).IsLessOrEqualTo(${value});"),
            
            // .Should().BeOfType<value>() -> Check.That(var).IsInstanceOfType(value);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\s*\(\s*\)\s*\.BeOfType\s*<\s*(?<value>[^\>]+)\s*>\s*\(\s*\)\s*;", "Check.That(${subject}).IsInstanceOfType(typeof(${value}));"),
            
            // .Should().BeEquivalentTo(object) -> Check.That(var).HasFieldsWithSameValues(object);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\s*\(\s*\)\s*\.BeEquivalentTo\s*\(\s*(?<value>(?:(?>[^(){}]+|\((?<Open>)|(?<-Open>\))|\{(?<Open>)|(?<-Open>\}))*(?(Open)(?!)))+?)\s*\)\s*(?:,\s*""[^""]*""\s*)?;","Check.That(${subject}).HasFieldsWithSameValues(${value});"),
            
            // .Should().Contain(value) -> Check.That(var).Contains(value);
            GetSubjectValueReplacement("Contain", "Check.That(${subject}).Contains(${value});"),
            
            // .Should().OnlyContain(value) -> Check.That(var).ContainsOnlyElementsThatMatch(value);
            GetSubjectValueReplacement("OnlyContain", "Check.That(${subject}).ContainsOnlyElementsThatMatch(${value});"),
            
            // .Should().HaveCount(value) -> Check.That(var).HasSize(value);
            GetSubjectValueReplacement("HaveCount", "Check.That(${subject}).HasSize(${value});"),
            
            // .Should().HaveSameCount(value) -> Check.That(var).HasSameSizeAs(value);
            GetSubjectValueReplacement("HaveSameCount", "Check.That(${subject}).HasSameSizeAs(${value});"),
            
            // .Should().HaveLength(value) -> Check.That(var).HasSize(value);
            GetSubjectValueReplacement("HaveLength", "Check.That(${subject}).HasSize(${value});"),
                
            // .Should().NotContain(value) -> Check.That(var).Not.Contains(value);
            GetSubjectValueReplacement("NotContain", "Check.That(${subject}).Not.Contains(${value});"),
            
            // .Should().BeEmpty() -> Check.That(var).IsEmpty();
            GetSubjectOnlyReplacement("BeEmpty", "Check.That(${subject}).IsEmpty();"),
                
            // .Should().NotBeEmpty() -> Check.That(var).IsNotEmpty();
            GetSubjectOnlyReplacement("NotBeEmpty", "Check.That(${subject}).Not.IsEmpty();"),
            
            // .Should().NotBeNullOrEmpty() -> Check.That(var).Not.IsNullOrEmpty();
            GetSubjectOnlyReplacement("NotBeNullOrEmpty", "Check.That(${subject}).Not.IsNullOrEmpty();"),
            
            // .Should().NotBeNullOrWhiteSpace() -> Check.That(var).Not.IsNullOrWhiteSpace();
            GetSubjectOnlyReplacement("NotBeNullOrWhiteSpace", "Check.That(${subject}).Not.IsNullOrWhiteSpace();"),
            
            // .Should().StarsWith(value) -> Check.That(var).StartsWith(value);
            GetSubjectValueReplacement("StartWith", "Check.That(${subject}).StartsWith(${value});"),
            
            // .Should().EndWith(value) -> Check.That(var).EndsWith(value);
            GetSubjectValueReplacement("EndWith", "Check.That(${subject}).EndsWith(${value});"),
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
        return ($@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.{Pattern}\s*\(\s*(?<value>(?:\(.*?\)|[^,()])+)\s*(?:,\s*.*)?\)\s*;", Replacement);
    }
    
    private static (string, string) GetSubjectOnlyReplacement(string Pattern, string Replacement)
    {
        return ($@"(?<subject>\S(?:.*\S)?)\s*\.Should\s*\(\s*\)\s*\.{Pattern}\s*\(\s*\)\s*(?:,\s*""[^""]*""\s*)?;", Replacement);
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
            GetSubjectOnlyReplacement("BeTrue","Check.That(${subject}).IsTrue();"),
                
            // .Should().BeFalse() -> Check.That(var).IsFalse();
            GetSubjectOnlyReplacement("BeFalse","Check.That(${subject}).IsFalse();"),
            
            // .Should().NotBeFalse() -> Check.That(var).Not.IsFalse();
            GetSubjectOnlyReplacement("NotBeFalse","Check.That(${subject}).Not.IsFalse();"),

            // .Should().NotBeTrue() -> Check.That(var).Not.IsTrue();
            GetSubjectOnlyReplacement("NotBeTrue","Check.That(${subject}).Not.IsTrue();"),

            // .Should().Imply(other) -> Check.That(var).Imply(other);
            GetSubjectValueReplacement("Imply","Check.That(${subject}).Imply(${value});"),
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
