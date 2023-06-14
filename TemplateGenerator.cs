using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XMLValueReplacer;

internal class TemplateGenerator
{
    public string FileName { get; } = "template.xml";
    public string Prefix { get; }
    public XPathOptionsEnum XPathOptions { get; }
    private XDocument Document { get; set; }

    public TemplateGenerator(XDocument document, string prefix, XPathOptionsEnum xPathOptions)
    {
        Document = document;
        Prefix = prefix;
        XPathOptions = xPathOptions;
    }

    public XDocument Generate()
    {
        var xmlInformation = new XmlInformation();

        CreateXPaths(xmlInformation, XPathOptions, Document);
        var namespaceManager = GetnamespaceManager(xmlInformation);

        var q = xmlInformation.NodeInformation.Select(x => x.FullNameXPath).GroupBy(x => x)
                      .Where(g => g.Count() > 1)
                      .Select(y => new { Element = y.Key, Counter = y.Count() }).ToList();

        var dupes = q.Select(x => x.Element);
        var remainingXPaths = xmlInformation.NodeInformation.Select(x => x.FullNameXPath).Except(dupes);

        foreach (var element in q)
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
                    var nodeInformation = xmlInformation.NodeInformation.FirstOrDefault(x => x.FullNameXPath == element.Element);

                    if (nodeInformation is null)
                        throw new NullReferenceException($"Could not find replacement value for XPath: //{element.Element}");

                    var replacement = nodeInformation.NameReplacement.Replace("}", $"{i + 1}}}");
                    node.SetValue(replacement);
                }
            }
        }

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

                node.SetValue(nodeInformation.NameReplacement);
            }
        }

        //doc.Save("template.xml");

        return Document;
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
                XPathOptionsEnum.ShortXPath => element.AncestorsAndSelf().Select(e => e.Name.LocalName.Length > 3 ? e.Name.LocalName.Substring(0, 3) : e.Name.LocalName).Reverse().ToList(),
                _ => throw new NotImplementedException(),
            };

            var fullNamePath = element.AncestorsAndSelf().Select(e => e.Name.ToString()).Reverse().ToList();

            var nameReplacement = string.Join("-", path);
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
                //var shortened = String.Format("{0:X}", ns.GetHashCode());
                var shortened = Helper.RandomString(3);
                namespaceMappings.TryAdd(ns, shortened);
            }

            foreach (var ns in xmlInformation.Namespaces)
            {
                var a = Regex.Replace(fullNameXPath, $@"{{{ns}}}", $"{namespaceMappings.GetValueOrDefault(ns)}:");
                fullNameXPath = fullNameXPath.Replace($"{{{ns}}}", $"{namespaceMappings.GetValueOrDefault(ns)}:");
            }

            var nodeInformation = new NodeInformation();

            nodeInformation.NameReplacement = $"{{{Prefix}{nameReplacement}}}";
            nodeInformation.FullNameXPath = fullNameXPath;

            xmlInformation.NamespaceMappings = namespaceMappings;
            xmlInformation.NodeInformation.Add(nodeInformation);
        }
    }
}

