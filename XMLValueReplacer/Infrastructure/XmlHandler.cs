using System.Xml;
using System.Xml.Linq;

namespace XMLValueReplacer.Infrastructure;

internal class XmlHandler : IXmlHandler
{
    private readonly IConsoleHandler _consoleHandler;

    public XmlHandler(IConsoleHandler consoleHandler)
    {
        _consoleHandler = consoleHandler;
    }

    public XDocument LoadXmlFromPath(string filePath)
    {
        XDocument? xml = null;
        try
        {
            xml = XDocument.Load(filePath);
        }
        catch (IOException e)
        {
            _consoleHandler.WriteError(e);
        }
        catch (XmlException e)
        {
            _consoleHandler.WriteError(e);
        }

        if (xml is null)
            _consoleHandler.WriteError("File was not found");

        return xml!;
    }
}

