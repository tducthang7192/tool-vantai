using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using Excels = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

namespace Transport
{
    class clsExcel
    {
        public DataTable ConnectCSV(string filetable)
        {
            Result clsResult = new Result();
            DataTable tb = new DataTable();
            try
            {
                DataSet ds = new DataSet();
               
                string gStrFol = returnFolder(filetable);
                string strCSVFile1 = ReturnFileName(filetable);
                string sConnection = string.Empty;
                string strSheet = string.Empty;
                DataTable dtTablesList = new DataTable();
                OleDbConnection oleExcelConnection;
                OleDbDataAdapter odaObj;

                sConnection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + gStrFol.Trim() + "\\" + strCSVFile1 +
                              ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1;CharacterSet=Unicode\"";

                oleExcelConnection = new OleDbConnection(sConnection);
                oleExcelConnection.Open();
                //Lấy tất cả thông tin trong excel (có bnhiêu sheet gồm cả tên sheet)

                dtTablesList = oleExcelConnection.GetSchema("Tables");

                // nếu file excel có dữ liệu thì thực hiện tiếp
                if (dtTablesList.Rows.Count > 0)
                {
                    // lấy tên sheet thứ 1 ( tên sheet lấy ở cột "TABLE_NAME")
                    strSheet = dtTablesList.Rows[0]["TABLE_NAME"].ToString();
                }

                dtTablesList.Clear();
                dtTablesList.Dispose();
                // Truy vấn tất cả dữ liệu từ sheet
                odaObj = new OleDbDataAdapter("SELECT * FROM [" + strSheet + "]", oleExcelConnection);
                // Đổ dữ liệu vào dataset              
                odaObj.Fill(ds);
                tb = ds.Tables[0];

                for (int i = tb.Rows.Count - 1; i >= 0; i--)
                {
                    if (tb.Rows[i][1] == DBNull.Value)
                        tb.Rows[i].Delete();
                }
                tb.AcceptChanges();

                //tb = ds.Tables[0];
                //tb = tb.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) ==0)).CopyToDataTable();
                oleExcelConnection.Close();

               // clsResult.DATASET = ds;
                //clsResult.RESULT = 1;
            }
            catch (Exception ex) //Error
            {
                clsResult.RESULT = 0;
                clsResult.MESSAGE = ex.ToString();
            }

            return tb;
        }

        private string returnFolder(string strPathFile)
        {
            string strCSVFile1 = "";
            int intLengthOfFileName1 = strPathFile.Trim().Length;
            int intLastIndex1 = strPathFile.Trim().LastIndexOf("\\");
            strCSVFile1 = strPathFile.Trim().Substring(0, intLastIndex1);
            return strCSVFile1;
        }

        private string ReturnFileName(string strPathFile)
        {
            string strCSVFile1 = "";
            int intLengthOfFileName1 = strPathFile.Trim().Length;
            int intLastIndex1 = strPathFile.Trim().LastIndexOf("\\");
            strCSVFile1 = strPathFile.Trim().Substring(intLastIndex1, intLengthOfFileName1 - intLastIndex1);

            strCSVFile1 = strCSVFile1.Remove(0, 1).Trim();
            return strCSVFile1;
        }

        public Result Export_To_Excel(System.Data.DataTable dt, string excelPath, string filename)
        {
            try
            {
                // load excel, and create a new workbook
                Excels.Application excelApp = new Excels.Application();
                excelApp.Workbooks.Add();

                // single worksheet
                Excels.Worksheet workSheet = (Excels.Worksheet) excelApp.ActiveSheet;

                Excels.Range range = (Excels.Range) workSheet.Cells.get_Range("A:F");
                range.NumberFormat = "@";

                range = (Excels.Range) workSheet.Cells.get_Range("G:H");
                range.NumberFormat = "0";

                // rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (i == 0)
                        {
                            workSheet.Cells[i + 1, j + 1] = dt.Columns[j].ColumnName;
                        }

                        workSheet.Cells[i + 2, j + 1] = dt.Rows[i][j];
                    }
                }

                excelPath += "/" + filename;
                workSheet.SaveAs(excelPath);
                excelApp.Quit();

                Result result = new Result(1, "Xuất file excel thành công");
                return result;
            }
            catch (Exception ex)
            {
                Result result = new Result(0, ex.ToString());
                return result;
            }
        }
    }
}
