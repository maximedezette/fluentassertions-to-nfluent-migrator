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
        var messageReplacer = new MessageReplacer();
        var generalReplacer = new GeneralReplacer();
        var importReplacer = new ImportReplacer();
        var nfluentReplacer = new NFluentMultipleAssertionReplacer();
        
        generalReplacer.SetNext(booleanReplacer);
        booleanReplacer.SetNext(stringReplacer);
        stringReplacer.SetNext(numericReplacer);
        numericReplacer.SetNext(exceptionReplacer );
        exceptionReplacer.SetNext(collectionReplacer);
        collectionReplacer.SetNext(messageReplacer);
        messageReplacer.SetNext(wildcardReplacer);
        wildcardReplacer.SetNext(importReplacer);
        importReplacer.SetNext(nfluentReplacer);

        content = generalReplacer.Handle(content);
        
        return content;
    }
}