using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Transactions;

namespace Transport
{
    //Class chứa các hàm statis sytem và database
    public class DHuy
    {
        public static string CNS = @"Data Source=192.168.3.156,1433;Initial Catalog=WMWTST;User ID=sa;password=cjgmd123!";
        public static int LastKQ = 0;
        public static string LastSQLString = "";
        public static string LastExceptionString = "";
        public static DataTable LastDataTableKQ = new DataTable();

        public static string LastSQLStringDebug = "";

        public DHuy()
        {

        }

        public static DataTable SELECT_NEWROW_FORSCHEMA(String TableName)
        {

            DataTable kq = new DataTable();
            SqlConnection con = new SqlConnection(CNS);
            String sql = "SELECT  TOP 0 * FROM " + TableName + "";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;


            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                kq.Load(dr);
                kq = TablePropertyRemove(kq);
                kq.Rows.Add(kq.NewRow());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }

            return kq;
        }
        public static DataTable SELECT_SQL(String SQL)
        {
            LastSQLString = SQL;
            LastExceptionString = "";
            DataTable kq = new DataTable("KQ");
            SqlConnection con = new SqlConnection(CNS);
            String sql = SQL;
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            LastSQLStringDebug = SQL;

            try
            {
                con.Open();
                SetAirFlow(con);
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                kq.Load(dr);
            }
            catch (Exception ex)
            {
                LastExceptionString = ex.ToString();
                // MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            LastDataTableKQ = kq;
            TablePropertyRemove(kq);
            return kq;
        }
        public static DataTable SELECT_SQL(String SQL,string connect)
        {
            LastSQLString = SQL;
            LastExceptionString = "";
            DataTable kq = new DataTable("KQ");
            SqlConnection con = new SqlConnection(connect);
            String sql = SQL;
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            LastSQLStringDebug = SQL;

            try
            {
                con.Open();
                SetAirFlow(con);
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                kq.Load(dr);
            }
            catch (Exception ex)
            {
                LastExceptionString = ex.ToString();
                // MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            LastDataTableKQ = kq;
            TablePropertyRemove(kq);
            return kq;
        }
        public static int DELETE(String TableName, String ID)
        {
            int kq = 0;
            SqlConnection con = new SqlConnection(CNS);
            String sql = String.Format("DELETE [" + TableName + "] WHERE REFERENCE_NUMBER = '{0}'",@ID);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@ID", ID));


            try
            {
                con.Open();
                kq = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return kq;
        }

        public static void SetAirFlow(SqlConnection con)
        {
            using (SqlCommand comm = new SqlCommand("SET ARITHABORT ON", con))
            { comm.ExecuteNonQuery(); }
        }
        public static DataTable TablePropertyRemove(DataTable dt)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].AllowDBNull = true;
                dt.Columns[i].ReadOnly = false;
            }

            return dt;
        }
        public static int INSERT_IDENTITY(String TableName, DataTable dt)
        {
            int kq = 0;
            String S1 = "";
            String S2 = "";

            SqlConnection con = new SqlConnection(CNS);
            SqlCommand cmd = new SqlCommand("", con);

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i == 0) continue;
                if (dt.Columns[i].ColumnName == "ADDDATE" || dt.Columns[i].ColumnName == "EDITDATE" || dt.Columns[i].ColumnName == "CDATETIME" || dt.Columns[i].ColumnName == "UDATETIME") continue;

                S1 = S1 == "" ? S1 + dt.Columns[i].ColumnName : S1 + "," + dt.Columns[i].ColumnName;
                S2 = S2 == "" ? S2 + "@" + dt.Columns[i].ColumnName : S2 + ",@" + dt.Columns[i].ColumnName;
                cmd.Parameters.Add(new SqlParameter("@" + dt.Columns[i].ColumnName, dt.Rows[0][i]));
            }
            cmd.CommandText = "INSERT INTO " + TableName + " (" + S1 + ") VALUES (" + S2 + ") ";

            try
            {
                con.Open();
                kq = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return kq;
        }




    }
}