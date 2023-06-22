using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using XMLValueReplacer.Domain.Entities;
using XMLValueReplacer.Domain.Enums;
using XMLValueReplacer.Common;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace XMLValueReplacer.Generators;

internal class ExcelGenerator : IFileGenerator
{
    public GeneratorStatusResponse Generate(ApplicationContext applicationContext)
    {
        Console.WriteLine("Generating excel file...");

        var status = new GeneratorStatusResponse();

        var filePath = Helper.CreateFilePath(FileType.Excel, applicationContext.FileName, applicationContext.OriginalFileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var values = applicationContext.XmlInformation.NodeInformation.Where(x => x.OriginalNodeValue != null).Select(x => (ReplacementValue: x.NameReplacement.Replace($"{{{applicationContext.Prefix}", "").Replace("}", ""), OriginalValue: x.OriginalNodeValue));

        try
        {
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
                    InlineString = new InlineString() { Text = new Text(value.ReplacementValue) }
                });

                originalValuesRow.Append(new Cell
                {
                    DataType = CellValues.InlineString,
                    InlineString = new InlineString() { Text = new Text(value.OriginalValue) }
                });
            }

            data.Append(replacementValuesRow);
            data.Append(originalValuesRow);

            sheets.Append(sheet);

            spreadsheet.Save();
            spreadsheet.Dispose();
        }
        catch (Exception e)
        {
            status.Exception = e;
            status.IsSuccessful = false;
        }

        return status;
    }
}

