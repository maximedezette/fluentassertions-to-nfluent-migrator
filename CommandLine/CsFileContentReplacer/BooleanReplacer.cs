using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public class BooleanReplacer: Handler
{
    /// <summary>
    /// Applies boolean-specific assertions replacement rules to the provided content.
    /// </summary>
    public override string Handle(string content)
    {
        var booleanReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().BeFalse() -> Check.That(var).IsFalse();
            GetSubjectOnlyReplacement("BeFalse", "Check.That(${subject}).IsFalse();"),
           
            // .Should().BeTrue() -> Check.That(var).IsTrue();
            GetSubjectOnlyReplacement("BeTrue", "Check.That(${subject}).IsTrue();"),
            
            // .Should().Imply(other) -> Check.That(var).Imply(other);
            GetSubjectValueReplacement("Imply", "Check.That(${subject}).Imply(${value});"),
            
            // .Should().NotBeFalse() -> Check.That(var).Not.IsFalse();
            GetSubjectOnlyReplacement("NotBeFalse", "Check.That(${subject}).Not.IsFalse();"),

            // .Should().NotBeTrue() -> Check.That(var).Not.IsTrue();
            GetSubjectOnlyReplacement("NotBeTrue", "Check.That(${subject}).Not.IsTrue();"),

        };

        foreach (var (pattern, replacement) in booleanReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return Next is not null ? Next.Handle(content) : content;
    }
}