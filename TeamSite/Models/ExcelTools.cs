using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Hosting;

namespace TeamSite.Models
{
    public class ExcelTools
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ExcelTools(ILogger logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public String[,] ExcelToStringArray(FileInfo filePath, String worksheetName)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage(filePath))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["DeploymentRequests"];
                    worksheet.Select();
                    int RowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;

                    String[,] data = new String[RowCount, ColCount];

                    for (int row = 1; row <= RowCount; row++)
                    {
                        for (int col = 1; col <= ColCount; col++)
                        {
                            if (worksheet.Cells[row, col].Text != null)
                            {
                                data[row - 1, col - 1] = worksheet.Cells[row, col].Text.ToString();
                            }
                            else
                            {
                                data[row - 1, col - 1] = "";
                            }
                        }
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error occured while importing." + ex.Message);
                return new String[0, 0];
            }
        }

        public List<String[]> FindRowsInDateRange(int columnNumber, String[,] data, DateTime startDate, DateTime endDate)
        {
            List<String[]> result = new List<string[]>();
            var numRows = data.GetLength(0);
            for (int i = 2; i < numRows; i++)
            {
                if (data[i, 8] != null && data[i, 8] != "" && Convert.ToDateTime(data[i, 8]) >= startDate && Convert.ToDateTime(data[i, 8]) <= endDate)
                {
                    String[] temp = new String[data.GetLength(1)];
                    for (int j = 0; j < temp.Length; j++)
                    {
                        temp[j] = data[i, j];
                    }
                    result.Add(temp);
                }
            }
            return result;
        }

        //public List<String[]> FilterOnExactValue(List<String[]> data, int column, String value)
        //{
        //    List<String[]> result = new List<string[]>();
        //    var numRows = data.Count;
        //    for (int i = 0; i < numRows; i++)
        //    {
        //        if (data[i, column] != null && data[i, 8] != "" && Convert.ToDateTime(data[i, 8]) >= startDate && Convert.ToDateTime(data[i, 8]) <= endDate)
        //        {
        //            String[] temp = new String[data.GetLength(1)];
        //            for (int j = 0; j < temp.Length; j++)
        //            {
        //                temp[j] = data[i, j];
        //            }
        //            result.Add(temp);
        //        }
        //    }
        //    return result;
        //}

        public void CopyFormToTemplate(FileInfo filePath)
        {
            try
            {
                ExcelPackage package = new ExcelPackage(filePath);

                ExcelWorksheet worksheet = package.Workbook.Worksheets["Template"];
                int RowCount = worksheet.Dimension.Rows;
                int ColCount = worksheet.Dimension.Columns;

                string templateRootFolder = _hostingEnvironment.WebRootPath + "\\templates\\";
                FileInfo formChange = new FileInfo(Path.Combine(templateRootFolder, "Form Change Template V1.xlsm"));

                // Copy form info to template
                ExcelPackage templatePackage = new ExcelPackage(formChange);
                templatePackage.Workbook.Worksheets.Add("Original", new ExcelPackage(filePath).Workbook.Worksheets["Template"]);

                // Run macro to populate master
                ExcelWorksheet control = templatePackage.Workbook.Worksheets["Control"];
                _logger.LogWarning(control.Workbook.CodeModule.ToString());

                templatePackage.Save();
            }
            catch(Exception ex)
            {
                _logger.LogError("Some error occured in CopyFormToTemplate.  " + ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
