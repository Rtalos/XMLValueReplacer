using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XMLValueReplacer;

internal class TemplateGenerator
{
    public string XmlFileName { get; } = "template.xml";
    public string TextFileName { get; } = "replacementvalues.txt";
    public string Prefix { get; }
    public XPathOptionsEnum XPathOptions { get; }
    private XDocument Document { get; }

    public TemplateGenerator(XDocument document, string prefix, XPathOptionsEnum xPathOptions)
    {
        if (document is null)
            throw new ArgumentNullException(nameof(document));

        Document = document;
        Prefix = prefix;
        XPathOptions = xPathOptions;
    }

    public (XDocument Document, string ReplacementValues) Generate()
    {
        var xmlInformation = new XmlInformation();

        CreateXPaths(xmlInformation, XPathOptions, Document);
        var namespaceManager = GetnamespaceManager(xmlInformation);

        var nodeObj = xmlInformation.NodeInformation.Select(x => x.FullNameXPath).GroupBy(x => x)
                      .Where(g => g.Count() > 1)
                      .Select(y => (Element: y.Key, Counter: y.Count())).ToList();

        var dupes = nodeObj.Select(x => x.Element);
        var remainingXPaths = xmlInformation.NodeInformation.Select(x => x.FullNameXPath).Except(dupes);

        ReplaceDoubles(xmlInformation, namespaceManager, nodeObj);
        ReplaceSingles(xmlInformation, namespaceManager, remainingXPaths);

        string replacementValues = Helper.GenerateTxtFile(xmlInformation, Prefix);

        return (Document, replacementValues);
    }

    private XmlNamespaceManager GetnamespaceManager(XmlInformation xmlInformation)
    {
        var manager = new XmlNamespaceManager(new NameTable());

        foreach (var ns in xmlInformation.Namespaces)
        {
            var shortened = xmlInformation.NamespaceMappings.GetValueOrDefault(ns);

            if (shortened is null)
                throw new NullReferenceException($"Cannot find namespace {ns} in the mapping object");

            manager.AddNamespace(shortened, ns);
        }
        return manager;
    }

    private void CreateXPaths(XmlInformation xmlInformation, XPathOptionsEnum xpathOptions, XDocument doc)
    {
        var namespaceMappings = new Dictionary<string, string>();

        foreach (var element in doc.Descendants())
        {
            List<string> path = xpathOptions switch
            {
                XPathOptionsEnum.XPath => element.AncestorsAndSelf().Select(e => e.Name.LocalName).Reverse().ToList(),
                XPathOptionsEnum.ShortXPath => element.AncestorsAndSelf().Select(e => e.Name.LocalName == doc.Root.Name.LocalName ? string.Empty : e.Name.LocalName).Reverse().ToList(),
                _ => throw new NotImplementedException(),
            };

            var fullNamePath = element.AncestorsAndSelf().Select(e => e.Name.ToString()).Reverse().ToList();

            var nameReplacement = string.Join("-", path).TrimStart('-');
            var fullNameXPath = string.Join("/", fullNamePath);

            var namespaces = Regex.Matches(fullNameXPath, @"\{(.+?)\}")
                                        .Cast<Match>()
                                        .Select(m => m.Groups[1].Value);

            foreach (var ns in namespaces)
            {
                xmlInformation.Namespaces.Add(ns);
            }

            foreach (var ns in xmlInformation.Namespaces)
            {
                var shortened = Helper.RandomString(3);
                namespaceMappings.TryAdd(ns, shortened);
            }

            foreach (var ns in xmlInformation.Namespaces)
            {
                var a = Regex.Replace(fullNameXPath, $@"{{{ns}}}", $"{namespaceMappings.GetValueOrDefault(ns)}:");
                fullNameXPath = fullNameXPath.Replace($"{{{ns}}}", $"{namespaceMappings.GetValueOrDefault(ns)}:");
            }

            var nodeInformation = new NodeInformation(fullNameXPath, $"{{{Prefix}{nameReplacement}}}");

            xmlInformation.NamespaceMappings = namespaceMappings;
            xmlInformation.NodeInformation.Add(nodeInformation);
        }
    }

    private void ReplaceDoubles(XmlInformation xmlInformation, XmlNamespaceManager namespaceManager, List<(string Element, int Counter)> nodeObj)
    {
        foreach (var element in nodeObj)
        {
            for (int i = 0; i < element.Counter; i++)
            {
                XElement? node = null;
                if (i == 0)
                {
                    node = Document.XPathSelectElement($"//{element.Element}", namespaceManager);
                }

                if (i > 0)
                {
                    node = Document.XPathSelectElement($"(//{element.Element})[{i + 1}]", namespaceManager);
                }

                if (node is null)
                    throw new NullReferenceException($"Could not find node with XPath: //{element.Element}");

                if (!node.HasElements)
                {
                    var nodeInformation = xmlInformation.NodeInformation.FirstOrDefault(x => x.ModifiedFullNameXPath == element.Element);

                    if (nodeInformation is null)
                        throw new NullReferenceException($"Could not find replacement value for XPath: //{element.Element}");

                    var replacement = nodeInformation.NameReplacement.Replace("}", $"{i + 1}}}");
                    nodeInformation.NameReplacement = replacement;
                    nodeInformation.ModifiedFullNameXPath = $"{nodeInformation.FullNameXPath}{i + 1}";

                    xmlInformation.ReplacementValues.Add(replacement);

                    node.SetValue(replacement);
                }
            }
        }
    }

    private void ReplaceSingles(XmlInformation xmlInformation, XmlNamespaceManager namespaceManager, IEnumerable<string> remainingXPaths)
    {
        foreach (var xpath in remainingXPaths)
        {
            var node = Document.XPathSelectElement($"//{xpath}", namespaceManager);

            if (node is null)
                throw new NullReferenceException($"Could not find node with XPath: //{xpath}");

            if (!node.HasElements)
            {
                var nodeInformation = xmlInformation.NodeInformation.FirstOrDefault(x => x.FullNameXPath == xpath);

                if (nodeInformation is null)
                    throw new NullReferenceException($"Could not find replacement value for XPath: //{xpath}");

                xmlInformation.ReplacementValues.Add(nodeInformation.NameReplacement);

                node.SetValue(nodeInformation.NameReplacement);
            }
        }
    }
}

