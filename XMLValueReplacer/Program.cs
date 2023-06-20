using System.Xml;
using System.Xml.Linq;
using XMLValueReplacer;

Console.WriteLine("Enter file path");
var filePathInput = Console.ReadLine();

if (string.IsNullOrEmpty(@filePathInput))
{
    Helper.WriteExceptionErrorMessage("No path was given");
}

XDocument? xml = null;
try
{
    xml = XDocument.Load(filePathInput!);
}
catch (IOException e)
{
    Helper.WriteExceptionErrorMessage(e);
}
catch (XmlException e)
{
    Helper.WriteExceptionErrorMessage(e);

}

if (xml is null)
    Helper.WriteExceptionErrorMessage("File was not found");

Console.WriteLine("Enter desired prefix (default is none)");
var prefixInput = Console.ReadLine() ?? string.Empty;

Console.WriteLine("Do you want replacement paths to be XPath or short XPath? (s or short for short XPaths, x or xpath for XPath. Default is XPath)");
var xpathOptionInput = Console.ReadLine();

xpathOptionInput = string.IsNullOrEmpty(xpathOptionInput) ? "x" : xpathOptionInput;

XPathOptionsEnum xpathOptions = xpathOptionInput.ToLowerInvariant() switch
{
    "x" => XPathOptionsEnum.XPath,
    "xpath" => XPathOptionsEnum.XPath,
    "s" => XPathOptionsEnum.ShortXPath,
    "short" => XPathOptionsEnum.ShortXPath,
    _ => throw new NotImplementedException(),
};

var generator = new TemplateGenerator(xml!, prefixInput, filePathInput!, xpathOptions);

var generated = generator.Generate();

var xmlFilePath = Helper.CreateFilePath(FileType.Xml, generator.FileName, generator.OriginalFileName);
var textFilePath = Helper.CreateFilePath(FileType.Txt, generator.TextFileName);

generated.Document.Save(xmlFilePath);

//File.WriteAllText(textFilePath, generated.ReplacementValues);

Console.WriteLine($"Successfully created template.xml at path: {Helper.GetFilePath(generator.FileName)}");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();