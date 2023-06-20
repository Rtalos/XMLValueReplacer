namespace XMLValueReplacer.Domain.Entities;

internal class XmlInformation
{
    public HashSet<string> Namespaces { get; set; } = new HashSet<string>();
    public Dictionary<string, string> NamespaceMappings { get; set; } = new Dictionary<string, string>();
    public List<NodeInformation> NodeInformation { get; set; } = new List<NodeInformation>();
    public List<string> ReplacementValues { get; set; } = new List<string>();
}

