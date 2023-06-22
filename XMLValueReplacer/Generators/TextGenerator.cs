using System.Runtime.CompilerServices;
using XMLValueReplacer.Domain.Entities;

[assembly: InternalsVisibleTo("Tests")]

namespace XMLValueReplacer.Generators;

internal class TextGenerator : IFileGenerator
{
    //TODO implement correctly
    public GeneratorStatusResponse Generate(ApplicationContext applicationContext)
    {
        Console.WriteLine("Generating text file...");

        var rv = applicationContext.XmlInformation.NodeInformation
                                             .Where(x => x.OriginalNodeValue != null)
                                             .Select(x => x.NameReplacement.Replace($"{{{applicationContext.Prefix}", "").Replace("}", ""));

        var txt = string.Join("\n", rv);

        txt.TrimStart().TrimEnd();
        //File.WriteAllText(textFilePath, generated.ReplacementValues);

        var status = new GeneratorStatusResponse();
        return status;
    }
}

