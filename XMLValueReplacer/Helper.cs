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

    internal static string GetCreatedFilePath(string filePath)
    {
        return Path.GetFullPath(filePath);
    }
}

