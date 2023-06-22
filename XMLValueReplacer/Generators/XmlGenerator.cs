using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;
using XMLValueReplacer.Common;
using XMLValueReplacer.Domain.Entities;
using XMLValueReplacer.Domain.Enums;
using System.Xml.XPath;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace XMLValueReplacer.Generators;

internal class XmlGenerator : IFileGenerator
{
    public GeneratorStatusResponse Generate(ApplicationContext applicationContext)
    {
        Console.WriteLine("Generating xml file...");

        var xmlInformation = new XmlInformation();
        var status = new GeneratorStatusResponse();

        CreateXPaths(xmlInformation, applicationContext.XPathOptions, applicationContext.Document, applicationContext.Prefix);
        var nameSpaceResponse = GetnamespaceManager(status, xmlInformation);

        if (!nameSpaceResponse.status.IsSuccessful)
            return status;

        var nodeObj = xmlInformation.NodeInformation.Select(x => x.FullNameXPath).GroupBy(x => x)
                      .Where(g => g.Count() > 1)
                      .Select(y => (Element: y.Key, Counter: y.Count())).ToList();

        var dupes = nodeObj.Select(x => x.Element);
        var remainingXPaths = xmlInformation.NodeInformation.Select(x => x.FullNameXPath).Except(dupes);

        if (!ReplaceDoubles(status, applicationContext.Document, xmlInformation, nameSpaceResponse.namespaceManager, nodeObj).IsSuccessful)
            return status;

        if (!ReplaceSingles(status, applicationContext.Document, xmlInformation, nameSpaceResponse.namespaceManager, remainingXPaths).IsSuccessful)
            return status;

        applicationContext.XmlInformation = xmlInformation;

        var xmlFilePath = Helper.CreateFilePath(FileType.Xml, applicationContext.FileName, applicationContext.OriginalFileName);
        applicationContext.Document.Save(xmlFilePath);

        return status;
    }

    private (XmlNamespaceManager namespaceManager, GeneratorStatusResponse status) GetnamespaceManager(GeneratorStatusResponse status, XmlInformation xmlInformation)
    {
        var manager = new XmlNamespaceManager(new NameTable());

        foreach (var ns in xmlInformation.Namespaces)
        {
            var shortened = xmlInformation.NamespaceMappings.GetValueOrDefault(ns);

            if (shortened is null)
                throw new NullReferenceException($"Cannot find namespace {ns} in the mapping object");

            manager.AddNamespace(shortened, ns);
        }
        return (manager, status);
    }

    private void CreateXPaths(XmlInformation xmlInformation, XPathOptions xpathOptions, XDocument doc, string prefix)
    {
        var namespaceMappings = new Dictionary<string, string>();

        foreach (var element in doc.Descendants())
        {
            List<string> path = xpathOptions switch
            {
                XPathOptions.XPath => element.AncestorsAndSelf().Select(e => e.Name.LocalName).Reverse().ToList(),
                XPathOptions.ShortXPath => element.AncestorsAndSelf().Select(e => e.Name.LocalName == doc?.Root?.Name.LocalName ? string.Empty : e.Name.LocalName).Reverse().ToList(),
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
                var shortened = RandomString(3);
                namespaceMappings.TryAdd(ns, shortened);
            }

            foreach (var ns in xmlInformation.Namespaces)
            {
                fullNameXPath = fullNameXPath.Replace($"{{{ns}}}", $"{namespaceMappings.GetValueOrDefault(ns)}:");
            }

            var nodeInformation = new NodeInformation(fullNameXPath, $"{{{prefix}{nameReplacement}}}");

            xmlInformation.NamespaceMappings = namespaceMappings;
            xmlInformation.NodeInformation.Add(nodeInformation);
        }
    }

    private GeneratorStatusResponse ReplaceDoubles(GeneratorStatusResponse status, XDocument document, XmlInformation xmlInformation, XmlNamespaceManager namespaceManager, List<(string Element, int Counter)> nodeObj)
    {
        foreach (var element in nodeObj)
        {
            for (int i = 0; i < element.Counter; i++)
            {
                XElement? node = null;
                if (i == 0)
                {
                    node = document.XPathSelectElement($"//{element.Element}", namespaceManager);
                }

                if (i > 0)
                {
                    node = document.XPathSelectElement($"(//{element.Element})[{i + 1}]", namespaceManager);
                }

                if (node is null)
                {
                    status.Exception = new NullReferenceException($"Could not find node with XPath: //{element.Element}");
                    status.IsSuccessful = false;
                    return status;
                }

                if (!node.HasElements)
                {
                    var nodeInformation = xmlInformation.NodeInformation.FirstOrDefault(x => x.ModifiedFullNameXPath == element.Element);

                    if (nodeInformation is null)
                    {
                        status.Exception = new NullReferenceException($"Could not find replaement value for XPath: //{element.Element}");
                        status.IsSuccessful = false;
                        return status;
                    }

                    var replacement = nodeInformation.NameReplacement.Replace("}", $"{i + 1}}}");
                    nodeInformation.NameReplacement = replacement;
                    nodeInformation.ModifiedFullNameXPath = $"{nodeInformation.FullNameXPath}{i + 1}";
                    nodeInformation.OriginalNodeValue = node.Value;

                    xmlInformation.ReplacementValues.Add(replacement);

                    node.SetValue(replacement);
                }
            }
        }

        return status;
    }

    private GeneratorStatusResponse ReplaceSingles(GeneratorStatusResponse status, XDocument document, XmlInformation xmlInformation, XmlNamespaceManager namespaceManager, IEnumerable<string> remainingXPaths)
    {
        foreach (var xpath in remainingXPaths)
        {
            var node = document.XPathSelectElement($"//{xpath}", namespaceManager);

            if (node is null)
            {
                status.Exception = new NullReferenceException($"Could not find node with XPath: //{xpath}");
                status.IsSuccessful = false;
                return status;
            }

            if (!node.HasElements)
            {
                var nodeInformation = xmlInformation.NodeInformation.FirstOrDefault(x => x.FullNameXPath == xpath);

                if (nodeInformation is null)
                {
                    status.Exception = new NullReferenceException($"Could not find replacement value for XPath: //{xpath}");
                    status.IsSuccessful = false;
                    return status;
                }

                nodeInformation.OriginalNodeValue = node.Value;

                xmlInformation.ReplacementValues.Add(nodeInformation.NameReplacement);

                node.SetValue(nodeInformation.NameReplacement);
            }
        }

        return status;
    }

    private string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

