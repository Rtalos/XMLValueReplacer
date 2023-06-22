using System.Xml.Linq;
using XMLValueReplacer.Domain.Enums;

namespace XMLValueReplacer.Domain.Entities;

internal class ApplicationContext
{
    public string FileName { get; } = "modified";
    public string TextFileName { get; } = "replacementvalues.txt";
    public string Prefix { get; }
    public string OriginalFileName { get; }
    public XmlInformation XmlInformation { get; set; } = default!;
    public XPathOptions XPathOptions { get; }
    public XDocument Document { get; }

    public ApplicationContext(XDocument document, string prefix, XPathOptions xPathOptions, string originalFileName)
    {
        Document = document;
        Prefix = prefix;
        XPathOptions = xPathOptions;
        OriginalFileName = originalFileName;
    }
}
