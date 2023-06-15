namespace XMLValueReplacer;

internal class NodeInformation
{
    public string FullNameXPath { get; set; } = default!;
    public string ModifiedFullNameXPath { get; set; } = default!;
    public string NameReplacement { get; set; } = default!;
    public NodeInformation(string fullNameXPath, string nameReplacement)
    {
        FullNameXPath = fullNameXPath;
        NameReplacement = nameReplacement;
        ModifiedFullNameXPath = fullNameXPath;
    }
}
