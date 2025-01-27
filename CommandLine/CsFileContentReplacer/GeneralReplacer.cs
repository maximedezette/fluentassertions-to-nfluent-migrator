using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public class GeneralReplacer: Handler
{
    public override string Handle(string content)
    {
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
            GetSubjectValueInDiamond("BeOfType", "Check.That(${subject}).IsInstanceOfType(typeof(${value}))"),

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

        return Next is not null ? Next.Handle(content) : content;
    }
}