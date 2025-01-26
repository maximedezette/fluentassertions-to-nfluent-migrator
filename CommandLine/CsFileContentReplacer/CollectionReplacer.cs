using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public partial class CsFileContentReplacer
{
    
    /// <summary>
    /// Applies collection-specific assertions replacement rules to the provided content.
    /// </summary>
    private static string ReplaceCollectionAssertions(string content)
    {
        var collectionReplacements = new (string Pattern, string Replacement)[]
        {
            // .Should().ContainSingle(value) -> Check.That(var.Count(value)).IsEqualTo(1);
            GetSubjectValueReplacement("ContainSingle", "Check.That(${subject}.Count(${value})).IsEqualTo(1);"),

            // .Should().ContainSingle(value) -> Check.That(var.Count(value)).IsEqualTo(1);
            GetSubjectOnlyReplacement("ContainSingle", "Check.That(${subject}).HasSize(1);"),

            // .Should().NotContainKey(value) -> Check.That(var.Keys).Not.Contains(value);
            GetSubjectValueReplacement("NotContainKey", "Check.That(${subject}.Keys).Not.Contains(${value});"),
            
            // .Should().OnlyContain(value) -> Check.That(var).ContainsOnlyElementsThatMatch(value);
            GetSubjectValueReplacement("OnlyContain",
                "Check.That(${subject}).ContainsOnlyElementsThatMatch(${value});"),

            // .Should().HaveCount(value) -> Check.That(var).HasSize(value);
            GetSubjectValueReplacement("HaveCount", "Check.That(${subject}).HasSize(${value});"),

            // .Should().HaveSameCount(value) -> Check.That(var).HasSameSizeAs(value);
            GetSubjectValueReplacement("HaveSameCount", "Check.That(${subject}).HasSameSizeAs(${value});"),
            // .Should().Contain(value) -> Check.That(var).Contains(value);
            GetSubjectValueReplacement("Contain", "Check.That(${subject}).Contains(${value});"),

            // .Should().ContainKey(value) -> Check.That(var.Keys).Contains(value);
            GetSubjectValueReplacement("ContainKey", "Check.That(${subject}.Keys).Contains(${value});"),
            
            
            // .Should().NotContain(value) -> Check.That(var).Not.Contains(value);
            GetSubjectValueReplacement("NotContain", "Check.That(${subject}).Not.Contains(${value});"),

            // .Should().BeEmpty() -> Check.That(var).IsEmpty();
            GetSubjectOnlyReplacement("BeEmpty", "Check.That(${subject}).IsEmpty();"),

            // .Should().NotBeEmpty() -> Check.That(var).IsNotEmpty();
            GetSubjectOnlyReplacement("NotBeEmpty", "Check.That(${subject}).Not.IsEmpty();"),
            
            // .Should().NotHaveValue() -> Check.That(var).Not.HasValue();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.NotHaveValue\s*\(\s*\)\s*;",
                "Check.That(${subject}).Not.HasValue();"),

            // .Should().HaveValue() -> Check.That(var).HasValue();
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.HaveValue\s*\(\s*\)\s*;",
                "Check.That(${subject}).HasValue();"),

            // .Should().Match(x => !x.HasValue || x > 0) -> Check.That(var).Matches(x => !x.HasValue || x > 0);
            (@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.Match\s*\(\s*(?<lambda>.+?)\s*\)\s*;",
                "Check.That(${subject}).Matches(${lambda});"),
            
            // .Should().HaveCountGreaterThanOrEqualTo(value) -> Check.That(var).WhoseSize().IsGreaterOrEqualThan(value);
            GetSubjectValueReplacement("HaveCountGreaterThanOrEqualTo",
                "Check.That(${subject}).WhoseSize().IsGreaterOrEqualThan(${value});"),

            // .Should().HaveCountGreaterThan(value) -> Check.That(var).WhoseSize().IsStrictlyGreaterThan(value);
            GetSubjectValueReplacement("HaveCountGreaterThan",
                "Check.That(${subject}).WhoseSize().IsStrictlyGreaterThan(${value});"),
            
            
            // .Should().BeNullOrEmpty() -> Check.That(var).IsNullOrEmpty();
            GetSubjectOnlyReplacement("BeNullOrEmpty", "Check.That(${subject}).IsNullOrEmpty();"),

            // .Should().NotBeNullOrEmpty() -> Check.That(var).Not.IsNullOrEmpty();
            GetSubjectOnlyReplacement("NotBeNullOrEmpty", "Check.That(${subject}).Not.IsNullOrEmpty();"),
        };

        foreach (var (pattern, replacement) in collectionReplacements)
        {
            content = Regex.Replace(content, pattern, replacement);
        }

        return content;
    }
}