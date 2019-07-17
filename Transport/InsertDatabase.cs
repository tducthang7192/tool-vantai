using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.Data.SqlClient;
using  System.Data;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace Transport
{
    class InsertDatabase
    {
        public HashSet<string> References;
        public DataTable MapingASN(DataTable data)
        {
            for (int i = 0; i < data.Columns.Count; i++)
            {
                if (data.Columns[i].ColumnName.Trim() == "REFERENCE_NUMBER") data.Columns[i].ColumnName = "REFERENCE_NUMBER";
                if (data.Columns[i].ColumnName.Trim() == "Order_Line_Id") data.Columns[i].ColumnName = "Order_Line_Id";
                if (data.Columns[i].ColumnName.Trim() == "Item_Code") data.Columns[i].ColumnName = "Item_Code";
                if (data.Columns[i].ColumnName.Trim() == "Quantity") data.Columns[i].ColumnName = "Quantity";
                if (data.Columns[i].ColumnName.Trim() == "Ship_From_Sub_Inventory") data.Columns[i].ColumnName = "Ship_From_Sub_Inventory";
                if (data.Columns[i].ColumnName.Trim() == "Group_Id") data.Columns[i].ColumnName = "Group_Id";
                if (data.Columns[i].ColumnName.Trim() == "Order_Line_Detail_Id") data.Columns[i].ColumnName = "Order_Line_Detail_Id";
            }
            return data;
        }
        public DataTable MapingSO(DataTable data)
        {
            for (int i = 0; i < data.Columns.Count; i++)
            {
                if (data.Columns[i].ColumnName.Trim() == "REFERENCE_NUMBER") data.Columns[i].ColumnName = "REFERENCE_NUMBER";
                if (data.Columns[i].ColumnName.Trim() == "SKU") data.Columns[i].ColumnName = "SKU";
                if (data.Columns[i].ColumnName.Trim() == "TRIP_CODE") data.Columns[i].ColumnName = "TRIP_CODE";
                if (data.Columns[i].ColumnName.Trim() == "NUMBER_TRIP") data.Columns[i].ColumnName = "NUMBER_TRIP";
                if (data.Columns[i].ColumnName.Trim() == "QTY") data.Columns[i].ColumnName = "QTY";
                if (data.Columns[i].ColumnName.Trim() == "SO") data.Columns[i].ColumnName = "SO";
            }
            return data;
        }
        public DataTable MapingChiaXe(DataTable data)
        {
            for (int i = 0; i < data.Columns.Count; i++)
            {
                //if (data.Columns[i].ColumnName.Trim() == "Order Number") data.Columns[i].ColumnName = "REFERENCE_NUMBER";
                //if (data.Columns[i].ColumnName.Trim() == "Ordered Item") data.Columns[i].ColumnName = "OrderedItem";
                //if (data.Columns[i].ColumnName.Trim() == "Trip") data.Columns[i].ColumnName = "Trip";
                //if (data.Columns[i].ColumnName.Trim() == "Code Trip") data.Columns[i].ColumnName = "CodeTrip";
                //if (data.Columns[i].ColumnName.Trim() == "Sum of Ordered Quantity") data.Columns[i].ColumnName = "QTY";
                //if (data.Columns[i].ColumnName.Trim() == "Shipping Instructions") data.Columns[i].ColumnName = "UOM";
                if (data.Columns[i].ColumnName.Trim() == "REFERENCE_NUMBER") data.Columns[i].ColumnName = "REFERENCE_NUMBER";
                if (data.Columns[i].ColumnName.Trim() == "SKU") data.Columns[i].ColumnName = "SKU";
                if (data.Columns[i].ColumnName.Trim() == "TRIP_CODE") data.Columns[i].ColumnName = "TRIP_CODE";
                if (data.Columns[i].ColumnName.Trim() == "NUMBER_TRIP") data.Columns[i].ColumnName = "NUMBER_TRIP";
                if (data.Columns[i].ColumnName.Trim() == "QTY") data.Columns[i].ColumnName = "QTY";
                if (data.Columns[i].ColumnName.Trim() == "SO") data.Columns[i].ColumnName = "SO";
                if (data.Columns[i].ColumnName.Trim() == "Date") data.Columns[i].ColumnName = "Date";
                if (data.Columns[i].ColumnName.Trim() == "So_Xe") data.Columns[i].ColumnName = "So_Xe";


            }
            return data;
        }

        public void DeleteDatabase(DataTable data)
        {
            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để import");
                return;
            }          
            DataTable dt=new DataTable();
            try
            {
                SqlConnection connec = new SqlConnection(Connection.connectionString);
                string sql = "select distinct REFERENCE_NUMBER from dbo.CHIAXE";
                connec.Open();
                SqlDataAdapter adapter =new SqlDataAdapter(sql,connec);
                adapter.Fill(dt);
                
                for (int i = 0; i < data.Rows.Count; i++)
                {
                     string REFEREN_EXCEL = DBase.StringReturn(data.Rows[i]["REFERENCE_NUMBER"]);
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                      string REFEREN_CHIAXE = DBase.StringReturn(dt.Rows[j]["REFERENCE_NUMBER"]);
                      if (REFEREN_CHIAXE == REFEREN_EXCEL)
                      {
                          DHuy.DELETE("CHIAXE", REFEREN_CHIAXE);
                      }
                    }
                }
                    connec.Close();
            }
            catch (Exception ex)  // thiếu trường colum
            {
                MessageBox.Show(ex.ToString());
            }


        }
        public void ImportIntoDatabase(DataTable data)
        {
            int count = data.Rows.Count;
            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để import");
                return;
            }
            DataTable dt = new DataTable();
            try
            {
                References =new HashSet<string>();
                foreach (DataRow r in data.Rows)
                {
              
                   string referen= DBase.StringReturn(r["REFERENCE_NUMBER"]);
                   References.Add(referen);

                }

                String a = "exec Test_ChiaXe [" + String.Join(", ", References) + "]";
                
                DataTable dtline = DHuy.SELECT_SQL(a);
              

            }
            catch (Exception ex)  // thiếu trường colum
            {
                MessageBox.Show(ex.ToString());
            }

        }      
        public void ImportDatabase(DataTable data)
        {
            SqlConnection connec = new SqlConnection(Connection.connectionString);

            try
            {
                connec.Open();
                SqlBulkCopy sqlBulk = new SqlBulkCopy(
                    connec,
                    SqlBulkCopyOptions.TableLock |

                    SqlBulkCopyOptions.FireTriggers |

                    SqlBulkCopyOptions.UseInternalTransaction,

                    null
                );
                sqlBulk.DestinationTableName = "CHIAXE";
                sqlBulk.WriteToServer(data);


                connec.Close();
                MessageBox.Show("DOne");
          
                {
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
               
            }
           
            //DataTable dt = new DataTable();
            //try
            //{
            //    SqlConnection connec = new SqlConnection(Connection.connectionString);
            //    string sql = "select top 0 * from dbo.CHIAXE";
            //    connec.Open();
            //    SqlDataAdapter adapter = new SqlDataAdapter(sql, connec);
            //    adapter.Fill(dt);
            //        foreach (DataRow rExcel in data.Rows)
            //        {
            //            if (rExcel["REFERENCE_NUMBER"] != "" || rExcel["REFERENCE_NUMBER"] != null)
            //            {
            //                DataRow drCurrent = dt.NewRow();
            //                drCurrent["REFERENCE_NUMBER"] = DBase.StringReturn(rExcel["REFERENCE_NUMBER"]);
            //                drCurrent["SKU"] = DBase.StringReturn(rExcel["SKU"]);
            //                drCurrent["TRIP_CODE"] = DBase.StringReturn(rExcel["TRIP_CODE"]);
            //                drCurrent["NUMBER_TRIP"] = DBase.StringReturn(rExcel["NUMBER_TRIP"]);
            //                drCurrent["QTY"] = DBase.isDouble(rExcel["QTY"]);
            //                drCurrent["SO"] = DBase.StringReturn(rExcel["SO"]);
            //                drCurrent["Date"] = DBase.StringReturn(rExcel["Date"]);
            //                drCurrent["SoXe"] = DBase.StringReturn(rExcel["So_Xe"]);
            //                dt.Rows.Add(drCurrent);
            //                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(adapter);
            //                adapter.Update(dt);
            //            }else
            //            {
            //                return;
            //            }
            //        }
            //        MessageBox.Show("Record Updated Successfully");
            //        connec.Close();               
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}

        }
    }
}
