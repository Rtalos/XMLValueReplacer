using XMLValueReplacer.Domain.Enums;

namespace XMLValueReplacer.Infrastructure;

internal class ConsoleHandler : IConsoleHandler
{
    public string SetPrefix()
    {
        Console.WriteLine("Enter desired prefix (default is none)");
        var prefixInput = Console.ReadLine() ?? string.Empty;
        return prefixInput;
    }
    public XPathOptions SetXPathOptions()
    {
        Console.WriteLine("Do you want replacement paths to be XPath or short XPath? (s or short for short XPaths, x or xpath for XPath. Default is XPath)");
        var xpathOptionInput = Console.ReadLine();

        xpathOptionInput = string.IsNullOrEmpty(xpathOptionInput) ? "x" : xpathOptionInput;

        XPathOptions xpathOptions = xpathOptionInput.ToLowerInvariant() switch
        {
            "x" => XPathOptions.XPath,
            "xpath" => XPathOptions.XPath,
            "s" => XPathOptions.ShortXPath,
            "short" => XPathOptions.ShortXPath,
            _ => throw new NotImplementedException(),
        };

        return xpathOptions;
    }

    public void ReadKey()
    {
        Console.ReadKey();
    }

    public string? ReadLine()
    {
        var input = Console.ReadLine();
        return input;
    }
    public string ReadFilePathInput()
    {
        Console.WriteLine("Enter file path");
        var filePathInput = Console.ReadLine();

        if (string.IsNullOrEmpty(@filePathInput))
        {
            WriteError("No path was given");
        }

        return filePathInput!;
    }

    public void WaitForExitKey()
    {
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    public void WriteError(string message)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();

        WaitForExitKey();

        Environment.Exit(0);
    }

    public void WriteError<TException>(TException exception) where TException : Exception
    {
        WriteError(exception.Message);
    }

    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }
}

