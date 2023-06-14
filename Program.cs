using System.Xml.Linq;
using XMLValueReplacer;

Console.WriteLine("Enter file path");
var filePathInput = Console.ReadLine();

if (string.IsNullOrEmpty(@filePathInput))
    throw new NullReferenceException("No path was given");

XDocument? xml;
try
{
    xml = XDocument.Load(filePathInput);
}
catch (IOException)
{
    throw new IOException("File was not found");
}

if (xml is null)
    throw new NullReferenceException("File was not found");

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

var generator = new TemplateGenerator(xml, prefixInput, xpathOptions);

var document = generator.Generate();

document.Save("template.xml");

Console.WriteLine($"Successfully created template.xml at path: {Helper.GetCreatedFilePath(generator.FileName)}");