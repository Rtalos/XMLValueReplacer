using XMLValueReplacer.Domain.Enums;

namespace XMLValueReplacer.Infrastructure;

internal interface IConsoleHandler
{
    string SetPrefix();
    XPathOptions SetXPathOptions();
    void ReadKey();
    string? ReadLine();
    string ReadFilePathInput();
    void WaitForExitKey();
    void WriteLine(string text);
    void WriteError(string message);
    void WriteError<TException>(TException exception) where TException : Exception;
}

