using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace XMLValueReplacer;

internal static class Helper
{
    internal const string xlsx = "xlsx";
    internal const string xml = "xml";
    internal const string txt = "txt";

    internal static string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

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
        if (!String.IsNullOrEmpty(originalFilename))
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

    internal static void WriteExceptionErrorMessage<TException>(TException exception) where TException : Exception
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine(exception.Message);
        Console.ResetColor();
        
        Environment.Exit(0);
    }
    
    internal static void WriteExceptionErrorMessage(string message)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();

        Environment.Exit(0);
    }

    internal static string GenerateTxtFile(XmlInformation xmlInformation, string prefix)
    {
        var rv = xmlInformation.NodeInformation
                                                .Where(x => x.OriginalNodeValue != null)
                                                .Select(x => x.NameReplacement.Replace($"{{{prefix}", "").Replace("}", ""));

        var txt = string.Join("\n", rv);

        return txt.TrimStart().TrimEnd();
    }

    internal static void GenerateExcelFile(XmlInformation xmlInformation, string fileName, string originalFileName, string prefix)
    {
        var filePath = CreateFilePath(FileType.Excel, fileName, originalFileName);
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        
        var values = xmlInformation.NodeInformation.Where(x => x.OriginalNodeValue != null).Select(x => (ReplacementValue: x.NameReplacement.Replace($"{{{prefix}", "").Replace("}", ""), OriginalValue:x.OriginalNodeValue));

        using var spreadsheet = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

        var workbookpart = spreadsheet.AddWorkbookPart();
        workbookpart.Workbook = new Workbook();
        var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
        SheetData data = new SheetData();
        worksheetPart.Worksheet = new Worksheet(data);

        Sheets sheets = spreadsheet.WorkbookPart!.Workbook.AppendChild(new Sheets());

        Sheet sheet = new Sheet()
        {
            Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart),
            SheetId = 1,
            Name = "Sheet1"
        };

        var replacementValuesRow = new Row();
        var originalValuesRow = new Row();

        foreach (var value in values)
        {
            replacementValuesRow.Append(new Cell
            {
                DataType = CellValues.InlineString,
                InlineString = new InlineString() { Text = new DocumentFormat.OpenXml.Spreadsheet.Text(value.ReplacementValue) }
            });

            originalValuesRow.Append(new Cell
            {
                DataType = CellValues.InlineString,
                InlineString = new InlineString() { Text = new DocumentFormat.OpenXml.Spreadsheet.Text(value.OriginalValue) }
            });
        }

        data.Append(replacementValuesRow);
        data.Append(originalValuesRow);

        sheets.Append(sheet);

        spreadsheet.Save();
        spreadsheet.Dispose();
    }
}

