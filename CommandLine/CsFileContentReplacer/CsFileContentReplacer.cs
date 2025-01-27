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
        var booleanReplacer = new BooleanReplacer();
        var stringReplacer = new StringReplacer();
        var numericReplacer = new NumericReplacer();
        var exceptionReplacer = new ExceptionReplacer();
        var collectionReplacer = new CollectionReplacer();
        var wildcardReplacer = new WildcardsReplacer();
        var generalReplacer = new GeneralReplacer();
        
        generalReplacer.SetNext(booleanReplacer);
        booleanReplacer.SetNext(stringReplacer);
        stringReplacer.SetNext(numericReplacer);
        numericReplacer.SetNext(exceptionReplacer );
        exceptionReplacer.SetNext(collectionReplacer);
        collectionReplacer.SetNext(wildcardReplacer);

        content = Regex.Replace(content, @"using\s+FluentAssertions(\.\w+)*\s*;", "using NFluent;");
        content = generalReplacer.Handle(content);
        
        return content;
    }
}