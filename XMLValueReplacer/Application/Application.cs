using XMLValueReplacer.Common;
using XMLValueReplacer.Domain.Entities;
using XMLValueReplacer.Generators;
using XMLValueReplacer.Infrastructure;

namespace XMLValueReplacer;

internal class Application
{
    private readonly IConsoleHandler _consoleHandler;
    private readonly IXmlHandler _xmlHandler;
    private readonly IFileGeneratorFactory _fileGeneratorFactory;

    public Application(IConsoleHandler consoleHandler, IXmlHandler xmlHandler, IFileGeneratorFactory fileGeneratorFactory)
    {
        _consoleHandler = consoleHandler;
        _xmlHandler = xmlHandler;
        _fileGeneratorFactory = fileGeneratorFactory;
    }

    internal void Run()
    {
        var applicationContext = CreateAppContext();

        var xmlGenerator = _fileGeneratorFactory.GetFileGenerator<XmlGenerator>();
        var excelGenerator = _fileGeneratorFactory.GetFileGenerator<ExcelGenerator>();

        var xmlStatus = xmlGenerator.Generate(applicationContext);
        if (!xmlStatus.IsSuccessful)
            _consoleHandler.WriteError_Exit0(xmlStatus.ErrorMessage);

        var excelStatus = excelGenerator.Generate(applicationContext);
        if (!excelStatus.IsSuccessful)
            _consoleHandler.WriteError_Exit0(xmlStatus.ErrorMessage);

        _consoleHandler.WriteLine($"Successfully created template.xml at path: {Helper.GetFilePath(applicationContext.FileName)}");
        _consoleHandler.WaitForExitKey();
    }

    private ApplicationContext CreateAppContext()
    {
        var filePathInput = _consoleHandler.ReadFilePathInput();
        var xml = _xmlHandler.LoadXmlFromPath(filePathInput);
        var prefixInput = _consoleHandler.SetPrefix();
        var xpathOptions = _consoleHandler.SetXPathOptions();
        var applicationContext = new ApplicationContext(xml, prefixInput, xpathOptions, Helper.GetFileNameFromPath(filePathInput));

        return applicationContext;
    }
}

