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
            GetSubjectOnlyReplacement("NotThrow", "Check.ThatCode(${subject}).DoesNotThrow();"),

            // .Should().NotThrowAsync() -> Check.ThatCode(action).DoesNotThrow();
            GetSubjectOnlyReplacement("NotThrowAsync", "Check.ThatCode(${subject}).DoesNotThrow();"),
            
            // .Should().ThrowAsync<ExceptionType>() -> Check.ThatCode(() => action()).ThrowsType(typeof(ExceptionType))
            GetSubjectValueInDiamond("ThrowAsync", "Check.ThatCode(() => ${subject}()).ThrowsType(typeof(${value}))"),
        };

        // Apply exception-specific replacements
        foreach (var (pattern, replacement) in exceptionReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }
        
        return Next is not null ? Next.Handle(content) : content;
    }
}