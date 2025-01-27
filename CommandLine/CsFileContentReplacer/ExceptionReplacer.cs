using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public class ExceptionReplacer: Handler
{
     /// <summary>
    /// Applies exception-specific assertions replacement rules to the provided content.
    /// </summary>
    public override string Handle(string content)
    {
        var exceptionReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().Throw<ExceptionType>()*; -> Check.ThatCode(action).Throws<ExceptionType>()*;
            GetSubjectValueInDiamond("Throw", "Check.ThatCode(${subject}).Throws<${value}>()"),

            // .Should().ThrowExactly<ExceptionType>()*; -> Check.ThatCode(action).ThrowsExactly<ExceptionType>()*;
            GetSubjectValueInDiamond("ThrowExactly", "Check.ThatCode(${subject}).Throws<${value}>()"),
            
            // .Should().NotThrow() -> Check.ThatCode(action).DoesNotThrow();
            (@"(?<action>\S(?:.*\S)?)\s*\.Should\(\)\s*\.NotThrow\s*\(\s*\)\s*;",
                "Check.ThatCode(${action}).DoesNotThrow();"),

            // .Should().NotThrowAsync() -> Check.ThatCode(action).DoesNotThrow();
            (@"(?:await\s+)?(?<action>\S(?:.*\S)?)\s*\.Should\(\)\s*\.NotThrowAsync\s*\(\s*\)\s*;",
                "Check.ThatCode(${action}).DoesNotThrow();"),

            // .Should().ThrowAsync<ExceptionType>().WithMessage("...") -> Check.ThatCode(() => action()).ThrowsType(typeof(ExceptionType)).WithMessage("...")
            (@"await\s+(?<action>\S(?:.*\S)?)\s*\.Should\(\)\s*\.ThrowAsync\s*<(?<exceptionType>[^>]+)>\s*\(\s*\)\s*\.WithMessage\((?<message>[^)]+)\)",
                "Check.ThatCode(() => ${action}()).ThrowsType(typeof(${exceptionType})).AndWhichMessage().Matches(${message})"),

            // .Should().ThrowAsync<ExceptionType>() -> Check.ThatCode(() => action()).ThrowsType(typeof(ExceptionType))
            (@"await\s+(?<action>\S(?:.*\S)?)\s*\.Should\(\)\s*\.ThrowAsync\s*<(?<exceptionType>[^>]+)>\s*\(\s*\)",
                "Check.ThatCode(() => ${action}()).ThrowsType(typeof(${exceptionType}))")
        };

        // Apply exception-specific replacements
        foreach (var (pattern, replacement) in exceptionReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }


        content = content.Replace(".WithMessage", ".AndWhichMessage().Matches");
        
        return Next is not null ? Next.Handle(content) : content;
    }
}