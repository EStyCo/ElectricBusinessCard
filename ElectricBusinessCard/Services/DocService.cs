using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ElectricBusinessCard.Models.Enums;
using ElectricBusinessCard.Services.EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectricBusinessCard.Services
{
    public class DocService(
        CategoryService _categoryService)
    {
        public async Task<FileContentResult> WriteDataInSamplePrice()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                if (categories is null || !categories.Any())
                    throw new Exception("Категории не найдены");

                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "PriceSample.xlsx");
                string outputFileName = $"PriceList_{DateTime.Now:dd.MM.yy}.xlsx";

                using (var workbook = new XLWorkbook(templatePath))
                {
                    var worksheet = workbook.Worksheet(1);
                    var columnWidths = SaveWidthColumns(worksheet);
                    int currentRow = 6;
                    int globalWorkNumber = 1;

                    foreach (var category in categories.OrderBy(c => c.CategoryIndex))
                    {
                        int worksCount = category.Works?.Count ?? 0;
                        if (worksCount == 0) continue;

                        AddHeader(worksheet, currentRow, category.Name);
                        currentRow++;
                        WriteData(category, worksheet, worksCount, currentRow, ref globalWorkNumber);
                        currentRow += worksCount;
                    }

                    SetWidthColumns(worksheet, columnWidths);
                    return CompletedFile(workbook, outputFileName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании файла: {ex.Message}", ex);
            }
        }

        private void AddHeader(IXLWorksheet worksheet, int currentRow, string categoryName)
        {
            var categoryHeaderRow = worksheet.Row(currentRow);
            categoryHeaderRow.Cell(2).Value = categoryName;

            worksheet.Range(currentRow, 2, currentRow, 7).Merge();

            var mergedRange = worksheet.Range(currentRow, 2, currentRow, 7);
            mergedRange.Style.Font.Bold = true;
            mergedRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            mergedRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            mergedRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            mergedRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            mergedRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        private void WriteData(CategoryWork category, IXLWorksheet worksheet, int worksCount, int currentRow, ref int globalWorkNumber)
        {
            var orderedWorks = category.Works.OrderBy(w => w.WorkIndex).ToList();

            for (int i = 0; i < worksCount; i++)
            {
                var work = orderedWorks[i];
                var row = worksheet.Row(currentRow + i);

                row.Cell(2).Value = $"{globalWorkNumber}.";
                row.Cell(3).Value = work.Name;
                row.Cell(4).Value = GetUnitDisplayName(work.Unit);
                row.Cell(5).Value = 0;
                row.Cell(6).Value = work.PriceInRubles;

                row.Cell(7).FormulaA1 = $"=E{currentRow + i}*F{currentRow + i}";

                var rowRange = worksheet.Range(currentRow + i, 2, currentRow + i, 7);
                rowRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                rowRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                globalWorkNumber++;
            }
        }

        private FileContentResult CompletedFile(XLWorkbook workbook, string outputFileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                var fileBytes = memoryStream.ToArray();

                return new FileContentResult(fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = outputFileName
                };
            }
        }

        private double[] SaveWidthColumns(IXLWorksheet worksheet)
        {
            var columnWidths = new double[worksheet.Columns().Count()];
            for (int col = 1; col <= worksheet.Columns().Count(); col++)
                columnWidths[col - 1] = worksheet.Column(col).Width;
            return columnWidths;
        }

        private void SetWidthColumns(IXLWorksheet worksheet, double[] columnWidths)
        {
            for (int col = 1; col <= columnWidths.Length; col++)
                worksheet.Column(col).Width = columnWidths[col - 1];
        }

        private string GetUnitDisplayName(UnitWorks unit)
        {
            return unit switch
            {
                UnitWorks.Piece => "шт.",
                UnitWorks.SquareMeter => "м²",
                UnitWorks.CubicMeter => "м³",
                _ => "шт."
            };
        }
    }
}