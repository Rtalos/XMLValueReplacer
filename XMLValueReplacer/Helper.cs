namespace XMLValueReplacer;

internal static class Helper
{
    internal static string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    internal static string GetFilePath(string filePath)
    {
        return Path.GetFullPath(filePath);
    }

    internal static string CreateFilePath(string fileName)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
    }
}

