using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TeamSite.Models
{
    public class Excel
    {
        //private readonly ILogger logger;
        private readonly IHostingEnvironment _he;
        private FileInfo _fileInfo;
        public ExcelWorksheet _ws;
        private ExcelPackage _pack;

        public Excel(FileInfo fileInfo, ILogger _logger, IHostingEnvironment hostingEnvironment)
        {
            _fileInfo = fileInfo;
            SetPack(fileInfo);
            _he = hostingEnvironment;
            //logger = _logger;
        }

        // Set this instance of the ExcelTools object to a specific excel file
        private void SetPack(FileInfo fileInfo)
        {
            _pack = new ExcelPackage(fileInfo);
        }

        // Set current active worksheet
        public void SetActiveWorkSheet(string wsName)
        {
            _ws = _pack.Workbook.Worksheets[wsName];
            _ws.Select();
        }

        public string GetCell(int row, int col)
        {
            return _ws.Cells[row, col].Text;
        }

        public void SaveAs(string name)
        {
            string sWebRootFolder = _he.WebRootPath + "/filesystem/res/";
            FileInfo fInfo = new FileInfo(Path.Combine(sWebRootFolder, name + ".xlsx"));
            _pack.SaveAs(fInfo);
        }

        public String[,] WsToStringArray()
        {
            int numCols = _ws.Dimension.Columns;
            int numRows = _ws.Dimension.Rows;

            String[,] array = new String[numRows, numCols];

            for (int row = 1; row <= numRows; row++)
            {
                for (int col = 1; col <= numCols; col++)
                {
                    if (_ws.Cells[row, col].Text != null)
                    {
                        array[row, col] = _ws.Cells[row, col].Text.ToString();
                    }
                    else
                    {
                        array[row, col] = "";
                    }
                }
            }

            return array;
        }

        // load files from fileSystem then get rows
        public String[] GetRow(int i, FileInfo filePath, String worksheetName)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage(filePath))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetName];
                    worksheet.Select();

                    int ColCount = worksheet.Dimension.Columns;

                    String[] result = new String[ColCount];

                    for (int col = 1; col <= ColCount; col++)
                    {
                        if (worksheet.Cells[i, col].Text != null)
                        {
                            result[col - 1] = worksheet.Cells[i, col].Text.ToString();
                        }
                        else
                        {
                            result[col - 1] = "";
                        }
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                //logger.LogCritical("Error in CopyRow: " + e.Message);
            }

            return new string[0];
        }

        public string[] GetRowFromArray(string[,] array, int row)
        {
            int length = array.GetLength(1);
            string[] result = new string[length];

            for (int col = 0; col < length; col++)
            {
                if (array[row, col] != null)
                {
                    result[col] = array[row, col];
                }
                else
                {
                    result[col] = "";
                }
            }
            return result;
        }

        public String[,] ExcelToStringArray(FileInfo filePath, String worksheetName)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage(filePath))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetName];
                    worksheet.Select();
                    int RowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;

                    //logger.LogCritical("Rows: " + RowCount + "  Cols: " + ColCount);

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
                //logger.LogError("Some error occured while importing: " + ex.Message);
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

        public void CopyFormToTemplate(FileInfo filePath)
        {
            try
            {
                ExcelPackage package = new ExcelPackage(filePath);

                ExcelWorksheet worksheet = package.Workbook.Worksheets["Template"];
                int RowCount = worksheet.Dimension.Rows;
                int ColCount = worksheet.Dimension.Columns;

                string templateRootFolder = _he.WebRootPath + "\\templates\\";
                FileInfo formChange = new FileInfo(Path.Combine(templateRootFolder, "Form Change Template V1.xlsm"));

                // Copy form info to template
                ExcelPackage templatePackage = new ExcelPackage(formChange);
                templatePackage.Workbook.Worksheets.Add("Original", new ExcelPackage(filePath).Workbook.Worksheets["Template"]);

                // Run macro to populate master
                ExcelWorksheet control = templatePackage.Workbook.Worksheets["Control"];
                //logger.LogWarning(control.Workbook.CodeModule.ToString());

                templatePackage.Save();
            }
            catch (Exception ex)
            {
                //logger.LogError("Some error occured in CopyFormToTemplate.  " + ex.Message + " " + ex.StackTrace);
            }
        }

        public int CastToInt(string input)
        {
            int result;
            if (!Int32.TryParse(input, out result))
            {
                result = -1;
            }
            return result;
        }
    }
}

