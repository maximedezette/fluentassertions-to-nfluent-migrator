namespace CommandLine.CsFileContentReplacer;

public interface IHandler
{
    void SetNext(IHandler handler);

    string Handle(string content);
}