using XMLValueReplacer.Domain.Enums;

namespace XMLValueReplacer.Common;

internal static class Helper
{
    private const string xlsx = "xlsx";
    private const string xml = "xml";
    private const string txt = "txt";

    internal static string GetFileNameFromPath(string path)
    {
        return Path.GetFileNameWithoutExtension(path);
    }

    internal static string GetFilePath(string filePath)
    {
        return Path.GetFullPath(filePath);
    }

    internal static string CreateFilePath(FileType fileType, string fileName, string originalFilename = "")
    {
        if (!string.IsNullOrEmpty(originalFilename))
        {
            fileName = $"{originalFilename}_{fileName}";
        }

        fileName = fileType switch
        {
            FileType.Excel => $"{fileName}.{xlsx}",
            FileType.Xml => $"{fileName}.{xml}",
            FileType.Txt => $"{fileName}.{txt}",
            _ => throw new NotImplementedException(),
        };

        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
    }
}

