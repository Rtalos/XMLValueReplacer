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
    void WriteError_Exit0(string message);
    void WriteError_Exit0<TException>(TException exception) where TException : Exception;
}

