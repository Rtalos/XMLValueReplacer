using System.Xml.Linq;

namespace XMLValueReplacer.Infrastructure;

internal interface IXmlHandler
{
    XDocument LoadXmlFromPath(string filePath);
}

