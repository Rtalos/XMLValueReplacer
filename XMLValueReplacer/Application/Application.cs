using XMLValueReplacer.Common;
using XMLValueReplacer.Domain.Enums;
using XMLValueReplacer.Infrastructure;

namespace XMLValueReplacer;

internal class Application
{
    private readonly IConsoleHandler _consoleHandler;
    private readonly IXmlHandler _xmlHandler;

    public Application(IConsoleHandler consoleHandler, IXmlHandler xmlHandler)
    {
        _consoleHandler = consoleHandler;
        _xmlHandler = xmlHandler;
    }

    internal void Run()
    {
        var filePathInput = _consoleHandler.ReadFilePathInput();

        var xml = _xmlHandler.LoadXmlFromPath(filePathInput);

        var prefixInput = _consoleHandler.SetPrefix();

        var xpathOptions = _consoleHandler.SetXPathOptions();

        var generator = new TemplateGenerator(xml, prefixInput, filePathInput!, xpathOptions);

        var generated = generator.Generate();

        var xmlFilePath = Helper.CreateFilePath(FileType.Xml, generator.FileName, generator.OriginalFileName);
        var textFilePath = Helper.CreateFilePath(FileType.Txt, generator.TextFileName);

        generated.Document.Save(xmlFilePath);

        //File.WriteAllText(textFilePath, generated.ReplacementValues);

        _consoleHandler.WriteLine($"Successfully created template.xml at path: {Helper.GetFilePath(generator.FileName)}");
        _consoleHandler.WaitForExitKey();
    }
}

