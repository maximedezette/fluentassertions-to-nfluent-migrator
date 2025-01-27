using System.Text.RegularExpressions;

namespace CommandLine.CsFileContentReplacer;

public class MessageReplacer: Handler
{
    public override string Handle(string content)
    {
        content = content.Replace(".WithMessage", ".AndWhichMessage().Matches");

        return Next is not null ? Next.Handle(content) : content;
    }
}