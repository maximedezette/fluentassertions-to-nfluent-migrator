namespace CommandLine.CsFileContentReplacer;

public abstract class Handler: IHandler
{
    protected IHandler? Next;
    
    public void SetNext(IHandler handler)
    {
        Next = handler;
    }

    public abstract string Handle(string content);
    
    protected static (string, string) GetSubjectValueReplacement(string Pattern, string Replacement)
    {
        return (
            $@"(?<subject>\S(?:.*?\S)?)\s*\.Should\(\)\s*\.{Pattern}\s*\(\s*(?<value>(""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*'|\((?:[^()]*|\((?:[^()]*|\([^()]*\))*\))*\)|[^,()])+?)\s*(?:,\s*""[^""]*"")?\)\s*;",
            Replacement);
    }


    protected static (string, string) GetSubjectOnlyReplacement(string Pattern, string Replacement)
    {
        return ($@"(?<subject>\S(?:.*\S)?)\s*\.Should\s*\(\s*\)\s*\.{Pattern}\s*\(\s*\)\s*(?:,\s*""[^""]*""\s*)?;",
            Replacement);
    }
    
    protected static (string, string) GetSubjectValueInDiamond(string Pattern, string Replacement)
    {
        return (
            $@"(?<subject>\S(?:.*?\S)?)\s*\.Should\s*\(\s*\)\s*\.{Pattern}\s*<(?<value>[^\>]+)>(\(\))?",
            Replacement
        );
    }
}