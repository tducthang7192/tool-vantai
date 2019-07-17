using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Transport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string path;
        clsExcel clsExcel = new clsExcel();
        DataTable DT = new DataTable();
        DataTable DTC = new DataTable();
        int totalrow = 0;
        int totalinsert = 0;
        int totalasn = 0;
        int totalasninsert = 0;
         int totalSOinsert = 0;
        int iteminsert = 0;
        string storerkey = "OW0302";    
        private string Warehouse = "wmwhse1";

        // click get path 
        private void btnImport_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                InsertDatabase Database = new InsertDatabase();
                path = openFileDialog1.FileName;
                txtFileName.Text = path;
                DT = ExcelToDS(path);
                DT = Database.MapingChiaXe(DT);
                dataGridView1.DataSource = DT;
                InsertDatabase dta = new InsertDatabase();
                dta.DeleteDatabase(DT);
                dta.ImportDatabase(DT);
                dta.ImportIntoDatabase(DT);
            }  
          
        }
 
        private void btnImport1_Click(object sender, EventArgs e)
        {
            InsertDatabase Database = new InsertDatabase();
            DT = ExcelToDS(path);
            DT = Database.MapingASN(DT);
            dataGridView1.DataSource = DT;
            Dowork();


        }
        // create asn on infor9
        private void Dowork()
        {
            try
            {
                string contold = "";
                string contnew = "";
                string receiptkey = "";
                int createnew = 0;

                int count = 0;

                totalrow = DT.Rows.Count;

                DataTable tbASN = DT.DefaultView.ToTable(true, "REFERENCE_NUMBER");
                totalasn = tbASN.Rows.Count;
                totalrow = DT.Rows.Count;
                foreach (DataRow r in DT.Rows)
                {
                    contnew = DBase.StringReturn(r["REFERENCE_NUMBER"]);
                    if (contold != contnew)
                    {
                        DataTable dt_row = DHuy.SELECT_NEWROW_FORSCHEMA(string.Format("{0}.RECEIPT", Warehouse));
                        DataTable dtasn = new DataTable();
                        dtasn = DHuy.SELECT_SQL(string.Format("CreateReceiptkeyforRF '{0}'", Warehouse));
                        if (dtasn.Rows.Count > 0)
                        {
                            receiptkey = DBase.StringReturn(dtasn.Rows[0]["RECEIPTKEY"]);
                        }

                        dt_row.Rows[0]["WHSEID"] = Warehouse;
                        dt_row.Rows[0]["RECEIPTKEY"] = receiptkey;
                        dt_row.Rows[0]["EXTERNRECEIPTKEY"] = "";
                        dt_row.Rows[0]["STORERKEY"] = storerkey;
                        dt_row.Rows[0]["CONTAINERKEY"] = "";
                        dt_row.Rows[0]["SUSR2"] = ""; // Seal No

                        dt_row.Rows[0]["STATUS"] = "0";
                        dt_row.Rows[0]["RECEIPTDATE"] = DateTime.Now;
                        dt_row.Rows[0]["EFFECTIVEDATE"] = DateTime.Now;
                        dt_row.Rows[0]["EXPECTEDRECEIPTDATE"] = DateTime.Now;
                        dt_row.Rows[0]["ArrivalDateTime"] = DateTime.Now;
                        dt_row.Rows[0]["ADVICEDATE"] = DateTime.Now;
                        dt_row.Rows[0]["FORTE_FLAG"] = "I";
                        dt_row.Rows[0]["OPENQTY"] = 0;
                        dt_row.Rows[0]["TRANSPORTATIONMODE"] = 1;
                        dt_row.Rows[0]["TYPE"] = 1;
                        dt_row.Rows[0]["RECEIPTGROUP"] = "";
                        dt_row.Rows[0]["ALLOWAUTORECEIPT"] = 0;
                        dt_row.Rows[0]["TRACKINVENTORYBY"] = 0;
                        dt_row.Rows[0]["LottableMatchRequired"] = 1;
                        dt_row.Rows[0]["ADDDATE"] = DateTime.Now;
                        dt_row.Rows[0]["ADDWHO"] = "DCCanTho";
                        dt_row.Rows[0]["EDITDATE"] = DateTime.Now;
                        dt_row.Rows[0]["EDITWHO"] = "DCCANTHO";

                        if (receiptkey != "")
                        {
                            createnew = DHuy.INSERT_IDENTITY(string.Format("{0}.RECEIPT", Warehouse), dt_row);
                            if (createnew == 0)
                            {
                                return;
                            }
                            else
                            {
                                totalasninsert++;

                            }
                        }
                    }

                    // Insert Detail
                    DataTable dt_detail = DHuy.SELECT_NEWROW_FORSCHEMA(string.Format("{0}.RECEIPTDETAIL", Warehouse));

                    DataTable dtline = new DataTable();
                    dtline = DHuy.SELECT_SQL(string.Format("CreateOrderLineASNforRF '{0}','" + receiptkey + "'", Warehouse));
                    string receipline = "";
                    if (dtline.Rows.Count > 0)
                    {
                        receipline = DBase.StringReturn(dtline.Rows[0]["RECEIPTLINENUMBER"]);
                    }

                    string sku = DBase.StringReturn(r["Item_Code"]);
                    DataTable dtsku = new DataTable();
                    dtsku = DHuy.SELECT_SQL(string.Format("SELECT * FROM {0}.SKU WHERE WHSEID='{0}' AND STORERKEY='" +storerkey + "' AND SKU='" + sku + "'", Warehouse));
                    if (dtsku.Rows.Count > 0)
                     {
                        string packkey = DBase.StringReturn(dtsku.Rows[0]["PACKKEY"]);
                        DataTable dtpack = new DataTable();
                        dtpack = DHuy.SELECT_SQL(string.Format( "SELECT * FROM {0}.PACK WHERE WHSEID='{0}' AND PACKKEY='" + packkey + "'", Warehouse));
                        dt_detail.Rows[0]["WHSEID"] = Warehouse;
                        dt_detail.Rows[0]["RECEIPTKEY"] = receiptkey;
                        dt_detail.Rows[0]["RECEIPTLINENUMBER"] = receipline;
                        dt_detail.Rows[0]["EXTERNRECEIPTKEY"] = "";
                        dt_detail.Rows[0]["EXTERNLINENO"] = "WMS" + receipline;
                        dt_detail.Rows[0]["SUBLINENUMBER"] = "";
                        dt_detail.Rows[0]["POKEY"] = "";
                        dt_detail.Rows[0]["STORERKEY"] = storerkey;
                        dt_detail.Rows[0]["STATUS"] = "0";
                        dt_detail.Rows[0]["TARIFFKEY"] = "XXXXXXXXXX";
                        dt_detail.Rows[0]["SKU"] = sku;
                        dt_detail.Rows[0]["ALTSKU"] = "";
                        dt_detail.Rows[0]["ID"] = "";
                        dt_detail.Rows[0]["DATERECEIVED"] = DateTime.Now;
                        dt_detail.Rows[0]["QTYEXPECTED"] = DBase.IntReturn(r["Quantity"]);
                        dt_detail.Rows[0]["QTYADJUSTED"] = 0;
                        dt_detail.Rows[0]["QTYRECEIVED"] = 0;

                        dt_detail.Rows[0]["UOM"] = dtpack.Rows[0]["PACKUOM3"];
                        dt_detail.Rows[0]["PACKKEY"] = packkey;

                        dt_detail.Rows[0]["TOLOC"] = "STAGE"; //DBase.StringReturn(r["Loc"]);
                        dt_detail.Rows[0]["TOLOT"] = "";
                        dt_detail.Rows[0]["TOID"] = receiptkey + receipline;
                        dt_detail.Rows[0]["CONDITIONCODE"] = "OK";

                        dt_detail.Rows[0]["LOTTABLE01"] = DBase.IntReturn(r["Group_Id"]);
                        dt_detail.Rows[0]["LOTTABLE02"] =  DBase.IntReturn(r["Order_Line_Id"]);
                        dt_detail.Rows[0]["LOTTABLE03"] = DBase.IntReturn(r["Order_Line_Detail_Id"]); 
                        // dt_detail.Rows[0]["LOTTABLE04"] = "";
                        //dt_detail.Rows[0]["LOTTABLE05"] = "";
                        dt_detail.Rows[0]["LOTTABLE06"] = "";
                        dt_detail.Rows[0]["LOTTABLE07"] = DBase.StringReturn(r["REFERENCE_NUMBER"]);
                        dt_detail.Rows[0]["LOTTABLE08"] = DBase.StringReturn(r["Ship_From_Sub_Inventory"]);
                        dt_detail.Rows[0]["LOTTABLE09"] = "";
                        dt_detail.Rows[0]["LOTTABLE10"] = receiptkey + receipline;
                        dt_detail.Rows[0]["CASECNT"] = 0;
                        dt_detail.Rows[0]["INNERPACK"] = 0;
                        dt_detail.Rows[0]["PALLET"] = 0;
                        dt_detail.Rows[0]["CUBE"] = 0;
                        dt_detail.Rows[0]["GROSSWGT"] = 0;
                        dt_detail.Rows[0]["NETWGT"] = 0;
                        dt_detail.Rows[0]["OTHERUNIT1"] = 0;
                        dt_detail.Rows[0]["OTHERUNIT2"] = 0;
                        dt_detail.Rows[0]["UNITPRICE"] = 0;
                        dt_detail.Rows[0]["EXTENDEDPRICE"] = 0;
                        dt_detail.Rows[0]["PACKINGSLIPQTY"] = 0;

                        dt_detail.Rows[0]["FORTE_FLAG"] = "I";
                        dt_detail.Rows[0]["EFFECTIVEDATE"] = DateTime.Now;
                        dt_detail.Rows[0]["TYPE"] = 1;

                        dt_detail.Rows[0]["QTYREJECTED"] = 0;
                        dt_detail.Rows[0]["QCREQUIRED"] = 0;
                        dt_detail.Rows[0]["QCSTATUS"] = "N";
                        dt_detail.Rows[0]["QCAUTOADJUST"] = 0;

                        dt_detail.Rows[0]["MatchLottable"] = 0;

                        dt_detail.Rows[0]["ADDDATE"] = DateTime.Now;
                        dt_detail.Rows[0]["ADDWHO"] = "DCCanTho";
                        dt_detail.Rows[0]["EDITDATE"] = DateTime.Now;
                        dt_detail.Rows[0]["EDITWHO"] = "DCCanTho";
                        if (receiptkey != "")
                        {
                            int rs = 0;
                            rs = DHuy.INSERT_IDENTITY(string.Format("{0}.RECEIPTDETAIL", Warehouse), dt_detail);
                            if (rs > 0)
                            {                              
                                totalinsert++;
                            }
                            else
                            {
                                MessageBox.Show("Không có dữ liệu");
                            }

                        }
                        else
                        {
                            MessageBox.Show("Không tạo được receipt");
                        }
                    }
                    contold = contnew;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //create so on infor9 
        private void SO_DoWork()
        {
            try
            {
                string orderold = "";
                string ordernew = "";
                string orderkey = "";
                int createnew = 0;
                int totalSO;
                totalrow = DT.Rows.Count;
                DataTable tbSO = DT.DefaultView.ToTable(true, "SO");
                totalSO = tbSO.Rows.Count;
                foreach (DataRow r in DT.Rows)
                {
                    ordernew = DBase.StringReturn(r["SO"]);
                    // Create New SO
                    if (orderold != ordernew)
                    {
                        // Create SO
                        DataTable dt_row = DHuy.SELECT_NEWROW_FORSCHEMA(string.Format("{0}.ORDERS", Warehouse));
                        DataTable dtso = new DataTable();
                        dtso = DHuy.SELECT_SQL(string.Format("CreateOrderskeyforRF '{0}'", Warehouse));

                        if (dtso.Rows.Count > 0)
                        {
                            orderkey = DBase.StringReturn(dtso.Rows[0]["ORDERKEY"]);
                        }

                        dt_row.Rows[0]["WHSEID"] = Warehouse;
                        dt_row.Rows[0]["ORDERKEY"] = orderkey;
                        dt_row.Rows[0]["EXTERNORDERKEY"] = DBase.IntReturn(r["TRIP_CODE"]); //"WMS" + orderkey;
                        dt_row.Rows[0]["STORERKEY"] = storerkey;
                        dt_row.Rows[0]["STATUS"] = "02";
                        dt_row.Rows[0]["ORDERDATE"] = DateTime.Now;
                        dt_row.Rows[0]["DELIVERYDATE"] = DateTime.Now;
                        dt_row.Rows[0]["PRIORITY"] = 5;
                        dt_row.Rows[0]["CONSIGNEEKEY"] = "";
                        dt_row.Rows[0]["C_COMPANY"] = "";
                        dt_row.Rows[0]["BATCHFLAG"] = 0;
                        dt_row.Rows[0]["UPDATESOURCE"] = 0;
                        dt_row.Rows[0]["TYPE"] = 0;
                        dt_row.Rows[0]["CONTAINERTYPE"] = "";
                        dt_row.Rows[0]["SUSR1"] = "";
                        dt_row.Rows[0]["SUSR3"] = "";
                        dt_row.Rows[0]["REFERENCENUM"] = DBase.StringReturn(r["NUMBER_TRIP"]);



                        dt_row.Rows[0]["OPENQTY"] = 0;
                        dt_row.Rows[0]["EFFECTIVEDATE"] = DateTime.Now;
                        dt_row.Rows[0]["FORTE_FLAG"] = "I";
                        dt_row.Rows[0]["SHIPTOGETHER"] = "N";
                        dt_row.Rows[0]["BILLTOKEY"] = "";
                        dt_row.Rows[0]["DOOR"] = "";
                        dt_row.Rows[0]["ROUTE"] = "";
                        dt_row.Rows[0]["INTERMODALVEHICLE"] = "";
                        dt_row.Rows[0]["INTERMODALVEHICLE"] = "";
                        dt_row.Rows[0]["ORDERGROUP"] = "";
                        dt_row.Rows[0]["ORDERVALUE"] = 0;

                        dt_row.Rows[0]["REQUESTEDSHIPDATE"] = DateTime.Now;
                        dt_row.Rows[0]["ACTUALSHIPDATE"] = DateTime.Now;
                        dt_row.Rows[0]["DELIVER_DATE"] = DateTime.Now;
                        dt_row.Rows[0]["EXTERNALORDERKEY2"] = DBase.StringReturn(r["REFERENCE_NUMBER"]); //"WMS" + orderkey;
                        

                        dt_row.Rows[0]["OHTYPE"] = 1;
                        dt_row.Rows[0]["RFIDFLAG"] = 0;
                        dt_row.Rows[0]["DepDateTime"] = DateTime.Now;

                        dt_row.Rows[0]["ALLOCATEDONERP"] = 0;
                        dt_row.Rows[0]["ENABLEPACKING"] = 0;
                        dt_row.Rows[0]["SUSPENDEDINDICATOR"] = 0;
                        dt_row.Rows[0]["AllowOverPick"] = 0;

                        dt_row.Rows[0]["ADDDATE"] = DateTime.Now;
                        dt_row.Rows[0]["ADDWHO"] = "DCCANTHO";
                        dt_row.Rows[0]["EDITDATE"] = DateTime.Now;
                        dt_row.Rows[0]["EDITWHO"] = "DCCANTHO";

                        if (orderkey != "")
                        {
                            createnew = DHuy.INSERT_IDENTITY(string.Format("{0}.ORDERS", Warehouse), dt_row);
                            if (createnew == 0)
                            {
                                break;
                            }
                            else
                            {
                                totalSOinsert++;

                            }
                        }
                    }

                    // Insert Detail
                    DataTable dt_detail = DHuy.SELECT_NEWROW_FORSCHEMA(string.Format("{0}.ORDERDETAIL", Warehouse));

                    DataTable dtline = new DataTable();
                    dtline = DHuy.SELECT_SQL(
                        string.Format("CreateOrderLineSOforRF '{0}','" + orderkey + "'", Warehouse));

                    string orderline = "";
                    if (dtline.Rows.Count > 0)
                    {
                        orderline = DBase.StringReturn(dtline.Rows[0]["ORDERLINENUMBER"]);
                    }

                    string sku = DBase.StringReturn(r["SKU"]);
                    DataTable dtsku = new DataTable();
                    dtsku = DHuy.SELECT_SQL(string.Format("SELECT * FROM {0}.SKU WHERE WHSEID='{0}' AND STORERKEY='" + storerkey + "' AND SKU='" + sku + "'", Warehouse));
                    if (dtsku.Rows.Count > 0)
                    {
                        string packkey = DBase.StringReturn(dtsku.Rows[0]["PACKKEY"]);
                        DataTable dtpack = new DataTable();
                        dtpack = DHuy.SELECT_SQL(string.Format( "SELECT * FROM {0}.PACK WHERE WHSEID='{0}' AND PACKKEY='" + packkey + "'", Warehouse));

                        dt_detail.Rows[0]["WHSEID"] = Warehouse;
                        dt_detail.Rows[0]["ORDERKEY"] = orderkey;
                        dt_detail.Rows[0]["ORDERLINENUMBER"] = orderline;

                        dt_detail.Rows[0]["EXTERNORDERKEY"] = ordernew; //"WMS" + orderkey;
                        dt_detail.Rows[0]["EXTERNLINENO"] = "WMS" + orderline;
                        dt_detail.Rows[0]["SKU"] = sku;
                        dt_detail.Rows[0]["MANUFACTURERSKU"] = "";
                        dt_detail.Rows[0]["RETAILSKU"] = "";
                        dt_detail.Rows[0]["ALTSKU"] = "";

                        dt_detail.Rows[0]["PICKCODE"] = "";
                        dt_detail.Rows[0]["CARTONGROUP"] = "";
                        dt_detail.Rows[0]["LOT"] = "";
                        dt_detail.Rows[0]["ID"] = "";
                        dt_detail.Rows[0]["FACILITY"] = "";

                        dt_detail.Rows[0]["UNITPRICE"] = 0;
                        dt_detail.Rows[0]["TAX01"] = 0;
                        dt_detail.Rows[0]["TAX02"] = 0;
                        dt_detail.Rows[0]["EXTENDEDPRICE"] = 0;
                        dt_detail.Rows[0]["PRODUCT_WEIGHT"] = 0;
                        dt_detail.Rows[0]["PRODUCT_CUBE"] = 0;
                        dt_detail.Rows[0]["ORIGCASEQTY"] = 0;
                        dt_detail.Rows[0]["ORIGPALLETQTY"] = 0;
                        dt_detail.Rows[0]["OKTOSUBSTITUTE"] = 0;
                        dt_detail.Rows[0]["ISSUBSTITUTE"] = 0;
                        dt_detail.Rows[0]["QTYINTRANSIT"] = 0;
                        dt_detail.Rows[0]["WPRELEASED"] = "";
                        dt_detail.Rows[0]["FULFILLQTY"] = 0;


                        dt_detail.Rows[0]["STORERKEY"] = storerkey;
                        dt_detail.Rows[0]["ORIGINALQTY"] = Math.Abs(DBase.IntReturn(r["QTY"]));
                        dt_detail.Rows[0]["OPENQTY"] = Math.Abs(DBase.IntReturn(r["QTY"]));

                        dt_detail.Rows[0]["SHIPPEDQTY"] = 0;
                        dt_detail.Rows[0]["ADJUSTEDQTY"] = 0;
                        dt_detail.Rows[0]["QTYPREALLOCATED"] = 0;
                        dt_detail.Rows[0]["QTYALLOCATED"] = 0;
                        dt_detail.Rows[0]["QTYPICKED"] = 0;

                        dt_detail.Rows[0]["UOM"] = dtpack.Rows[0]["PACKUOM3"];
                        dt_detail.Rows[0]["PACKKEY"] = packkey;
                        dt_detail.Rows[0]["STATUS"] = "02";
                        dt_detail.Rows[0]["UPDATESOURCE"] = 0;

                        dt_detail.Rows[0]["LOTTABLE01"] = "";
                        dt_detail.Rows[0]["LOTTABLE02"] = "";
                        dt_detail.Rows[0]["LOTTABLE03"] = "";
                        //dt_detail.Rows[0]["LOTTABLE04"] = DBase.DatetimeReturn(r["ExpectedDate"]);
                        //dt_detail.Rows[0]["LOTTABLE05"] = DBase.DatetimeReturn(r["ActualShipDate"]);
                        dt_detail.Rows[0]["LOTTABLE06"] = "";
                        dt_detail.Rows[0]["LOTTABLE07"] = DBase.StringReturn(r["REFERENCE_NUMBER"]);
                        dt_detail.Rows[0]["LOTTABLE08"] = "";
                        dt_detail.Rows[0]["LOTTABLE09"] = "";
                        dt_detail.Rows[0]["LOTTABLE10"] = "";

                        dt_detail.Rows[0]["FORTE_FLAG"] = "I";
                        dt_detail.Rows[0]["EFFECTIVEDATE"] = DateTime.Now;
                        dt_detail.Rows[0]["TARIFFKEY"] = "XXXXXXXXXX";
                        //DBase.StringReturn(dtsku.Rows[0]["STRATEGYKEY"])
                        dt_detail.Rows[0]["ALLOCATESTRATEGYKEY"] = "GMDSTGY";
                        dt_detail.Rows[0]["PREALLOCATESTRATEGYKEY"] ="" ;
                        dt_detail.Rows[0]["ALLOCATESTRATEGYTYPE"] = 1;
                        dt_detail.Rows[0]["SKUROTATION"] = "Lottable04";

                        dt_detail.Rows[0]["SHELFLIFE"] = 0;
                        dt_detail.Rows[0]["ROTATION"] = DBase.StringReturn(dtsku.Rows[0]["DEFAULTROTATION"]);
                        dt_detail.Rows[0]["SHIPGROUP01"] = "N";
                        dt_detail.Rows[0]["SHIPGROUP02"] = "N";
                        dt_detail.Rows[0]["SHIPGROUP03"] = "N";
                        dt_detail.Rows[0]["CARTONQTYBREAK"] = 0;
                        dt_detail.Rows[0]["GenerateContainerDetail"] = 0;

                        dt_detail.Rows[0]["MINSHIPPERCENT"] = 0;
                        dt_detail.Rows[0]["OQCREQUIRED"] = 0;
                        dt_detail.Rows[0]["OQCAUTOADJUST"] = 0;

                        dt_detail.Rows[0]["ADDDATE"] = DateTime.Now;
                        dt_detail.Rows[0]["ADDWHO"] = "DCCANTHO";
                        dt_detail.Rows[0]["EDITDATE"] = DateTime.Now;
                        dt_detail.Rows[0]["EDITWHO"] = "DCCANTHO";

                        if (orderkey != "")
                        {
                            int kq = DHuy.INSERT_IDENTITY(string.Format("{0}.ORDERDETAIL", Warehouse), dt_detail);
                            if (kq > 0)
                            {

                                totalinsert++;
                            }
                            else
                            {
                                MessageBox.Show("Không Có Dữ Liệu ");
                            }
                        }
                    }
                   
                    orderold = ordernew;
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        //function get data from file excel
        public DataTable ExcelToDS(string Path)
        {
            clsExcel clsExcel=new clsExcel();
            DataTable dt = null;
            try
            {
                dt = clsExcel.ConnectCSV(Path);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                
            }
            return dt;

        }
        private void btnImportSO_Click(object sender, EventArgs e)
        {
            InsertDatabase Database = new InsertDatabase();
            DT = ExcelToDS(path);
            DT = Database.MapingSO(DT);
            dataGridView1.DataSource = DT;
            SO_DoWork();
        }

        private void Form_Load()
        {
            string contold = "";
            string contnew = "";
            string receiptkey = "";
            int createnew = 0;
            DataTable DT_EDI_DETAIL = new DataTable();
            DataTable DT_EDI_HEADER = new DataTable();
            DataTable RECEIPTS = new DataTable();

            DT_EDI_DETAIL = DHuy.SELECT_SQL(string.Format("CreateReceiptkeyforRF '{0}'", Warehouse));
            DT_EDI_HEADER = DHuy.SELECT_SQL(string.Format("CreateReceiptkeyforRF '{0}'", Warehouse));
            foreach (DataRow r in DT_EDI_HEADER.Rows )
            {  //nếu header có trạng thái khác ok thì mới thực hiện
                    string STATUS = DBase.StringReturn(r["STATUS"]);
                    RECEIPTS = DHuy.SELECT_SQL(string.Format("SELECT DISTINCT RECEIPTKEY FROM {0}.RECEIPT WHERE SUSR1='{1}'", Warehouse, DBase.StringReturn(r["REFERENCE_NUMBER"])));
                    string receiptkey1 = RECEIPTS.Rows[0][0].ToString();
                    if (receiptkey1 == "")
                    {
                        DataTable dtasn = new DataTable();
                        dtasn = DHuy.SELECT_SQL(string.Format("CreateReceiptkeyforRF '{0}'", Warehouse));
                        if (dtasn.Rows.Count > 0)
                        {
                            receiptkey = DBase.StringReturn(dtasn.Rows[0]["RECEIPTKEY"]);
                        }

                        foreach (DataRow rd in DT_EDI_DETAIL.Rows)
                        {
                            if (DBase.StringReturn(rd["REFERENCE_NUMBER"]) == DBase.StringReturn(r["REFERENCE_NUMBER"]) && DBase.StringReturn(rd["STATUS"]) != "OK")
                            {

                                DataTable dt_detail =
                                    DHuy.SELECT_NEWROW_FORSCHEMA(string.Format("{0}.RECEIPTDETAIL", Warehouse));
                                DataTable dtline = new DataTable();
                                dtline = DHuy.SELECT_SQL(
                                    string.Format("CreateOrderLineASNforRF '{0}','" + receiptkey + "'", Warehouse));
                                string receipline = "";
                                if (dtline.Rows.Count > 0)
                                {
                                    receipline = DBase.StringReturn(dtline.Rows[0]["RECEIPTLINENUMBER"]);
                                }

                                string sku = DBase.StringReturn(rd["Item_Code"]);
                                DataTable dtsku = new DataTable();
                                dtsku = DHuy.SELECT_SQL(string.Format(
                                    "SELECT * FROM {0}.SKU WHERE WHSEID='{0}' AND STORERKEY='" + storerkey +
                                    "' AND SKU='" + sku + "'", Warehouse));
                                if (dtsku.Rows.Count > 0)
                                {
                                    string packkey = DBase.StringReturn(dtsku.Rows[0]["PACKKEY"]);
                                    DataTable dtpack = new DataTable();
                                    dtpack = DHuy.SELECT_SQL(string.Format(
                                        "SELECT * FROM {0}.PACK WHERE WHSEID='{0}' AND PACKKEY='" + packkey + "'",
                                        Warehouse));
                                    dt_detail.Rows[0]["WHSEID"] = Warehouse;
                                    dt_detail.Rows[0]["RECEIPTKEY"] = receiptkey;
                                    dt_detail.Rows[0]["RECEIPTLINENUMBER"] = receipline;
                                    dt_detail.Rows[0]["EXTERNRECEIPTKEY"] = "";
                                    dt_detail.Rows[0]["EXTERNLINENO"] = "WMS" + receipline;
                                    dt_detail.Rows[0]["SUBLINENUMBER"] = "";
                                    dt_detail.Rows[0]["POKEY"] = "";
                                    dt_detail.Rows[0]["STORERKEY"] = storerkey;
                                    dt_detail.Rows[0]["STATUS"] = "0";
                                    dt_detail.Rows[0]["TARIFFKEY"] = "XXXXXXXXXX";
                                    dt_detail.Rows[0]["SKU"] = sku;
                                    dt_detail.Rows[0]["ALTSKU"] = "";
                                    dt_detail.Rows[0]["ID"] = "";
                                    dt_detail.Rows[0]["DATERECEIVED"] = DateTime.Now;
                                    dt_detail.Rows[0]["QTYEXPECTED"] = DBase.IntReturn(rd["Quantity"]);
                                    dt_detail.Rows[0]["QTYADJUSTED"] = 0;
                                    dt_detail.Rows[0]["QTYRECEIVED"] = 0;

                                    dt_detail.Rows[0]["UOM"] = dtpack.Rows[0]["PACKUOM3"];
                                    dt_detail.Rows[0]["PACKKEY"] = packkey;

                                    dt_detail.Rows[0]["TOLOC"] = "STAGE"; //DBase.StringReturn(r["Loc"]);
                                    dt_detail.Rows[0]["TOLOT"] = "";
                                    dt_detail.Rows[0]["TOID"] = receiptkey + receipline;
                                    dt_detail.Rows[0]["CONDITIONCODE"] = "OK";

                                    dt_detail.Rows[0]["LOTTABLE01"] = DBase.IntReturn(rd["Group_Id"]);
                                    dt_detail.Rows[0]["LOTTABLE02"] = DBase.IntReturn(rd["Order_Line_Id"]);
                                    dt_detail.Rows[0]["LOTTABLE03"] = DBase.IntReturn(rd["Order_Line_Detail_Id"]);
                                    // dt_detail.Rows[0]["LOTTABLE04"] = "";
                                    //dt_detail.Rows[0]["LOTTABLE05"] = "";
                                    dt_detail.Rows[0]["LOTTABLE06"] = "";
                                    dt_detail.Rows[0]["LOTTABLE07"] = DBase.StringReturn(rd["REFERENCE_NUMBER"]);
                                    dt_detail.Rows[0]["LOTTABLE08"] = DBase.StringReturn(rd["Ship_From_Sub_Inventory"]);
                                    dt_detail.Rows[0]["LOTTABLE09"] = "";
                                    dt_detail.Rows[0]["LOTTABLE10"] = receiptkey + receipline;
                                    dt_detail.Rows[0]["CASECNT"] = 0;
                                    dt_detail.Rows[0]["INNERPACK"] = 0;
                                    dt_detail.Rows[0]["PALLET"] = 0;
                                    dt_detail.Rows[0]["CUBE"] = 0;
                                    dt_detail.Rows[0]["GROSSWGT"] = 0;
                                    dt_detail.Rows[0]["NETWGT"] = 0;
                                    dt_detail.Rows[0]["OTHERUNIT1"] = 0;
                                    dt_detail.Rows[0]["OTHERUNIT2"] = 0;
                                    dt_detail.Rows[0]["UNITPRICE"] = 0;
                                    dt_detail.Rows[0]["EXTENDEDPRICE"] = 0;
                                    dt_detail.Rows[0]["PACKINGSLIPQTY"] = 0;

                                    dt_detail.Rows[0]["FORTE_FLAG"] = "I";
                                    dt_detail.Rows[0]["EFFECTIVEDATE"] = DateTime.Now;
                                    dt_detail.Rows[0]["TYPE"] = 1;

                                    dt_detail.Rows[0]["QTYREJECTED"] = 0;
                                    dt_detail.Rows[0]["QCREQUIRED"] = 0;
                                    dt_detail.Rows[0]["QCSTATUS"] = "N";
                                    dt_detail.Rows[0]["QCAUTOADJUST"] = 0;

                                    dt_detail.Rows[0]["MatchLottable"] = 0;

                                    dt_detail.Rows[0]["ADDDATE"] = DateTime.Now;
                                    dt_detail.Rows[0]["ADDWHO"] = "DCCanTho";
                                    dt_detail.Rows[0]["EDITDATE"] = DateTime.Now;
                                    dt_detail.Rows[0]["EDITWHO"] = "DCCanTho";
                                    if (receiptkey != "")
                                    {
                                        int rs = 0;
                                        rs = DHuy.INSERT_IDENTITY(string.Format("{0}.RECEIPTDETAIL", Warehouse),
                                            dt_detail);
                                        if (rs > 0)
                                        {
                                            totalinsert++;
                                        }
                                        else
                                        {
                                            MessageBox.Show("Không có dữ liệu");
                                        }

                                    }
                                    else
                                    {
                                        MessageBox.Show("Không tạo được receipt");
                                    }
                                }
                            }

                        }

                        if (STATUS != "OK")
                        {
                            DataTable dt_row = DHuy.SELECT_NEWROW_FORSCHEMA(string.Format("{0}.RECEIPT", Warehouse));
                            dt_row.Rows[0]["WHSEID"] = Warehouse;
                            dt_row.Rows[0]["RECEIPTKEY"] = receiptkey;
                            dt_row.Rows[0]["EXTERNRECEIPTKEY"] = "";
                            dt_row.Rows[0]["STORERKEY"] = storerkey;
                            dt_row.Rows[0]["CONTAINERKEY"] = "";
                            dt_row.Rows[0]["SUSR2"] = ""; // Seal No

                            dt_row.Rows[0]["STATUS"] = "0";
                            dt_row.Rows[0]["RECEIPTDATE"] = DateTime.Now;
                            dt_row.Rows[0]["EFFECTIVEDATE"] = DateTime.Now;
                            dt_row.Rows[0]["EXPECTEDRECEIPTDATE"] = DateTime.Now;
                            dt_row.Rows[0]["ArrivalDateTime"] = DateTime.Now;
                            dt_row.Rows[0]["ADVICEDATE"] = DateTime.Now;
                            dt_row.Rows[0]["FORTE_FLAG"] = "I";
                            dt_row.Rows[0]["OPENQTY"] = 0;
                            dt_row.Rows[0]["TRANSPORTATIONMODE"] = 1;
                            dt_row.Rows[0]["TYPE"] = 1;
                            dt_row.Rows[0]["RECEIPTGROUP"] = "";
                            dt_row.Rows[0]["ALLOWAUTORECEIPT"] = 0;
                            dt_row.Rows[0]["TRACKINVENTORYBY"] = 0;
                            dt_row.Rows[0]["LottableMatchRequired"] = 1;
                            dt_row.Rows[0]["ADDDATE"] = DateTime.Now;
                            dt_row.Rows[0]["ADDWHO"] = "DCCanTho";
                            dt_row.Rows[0]["EDITDATE"] = DateTime.Now;
                            dt_row.Rows[0]["EDITWHO"] = "DCCANTHO";

                            if (receiptkey != "")
                            {
                                createnew = DHuy.INSERT_IDENTITY(string.Format("{0}.RECEIPT", Warehouse), dt_row);
                                if (createnew == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    totalasninsert++;

                                }
                            }

                        }
                    }
                    else
                    {
                    foreach (DataRow rd in DT_EDI_DETAIL.Rows)
                    {// tạo 1 line mới bằng thứ tự line đã có +1
                       
                        if (DBase.StringReturn(rd["REFERENCE_NUMBER"]) == DBase.StringReturn(r["REFERENCE_NUMBER"]) && DBase.StringReturn(rd["STATUS"]) != "OK")
                        {
                            int receipline = 0;
                            DataTable dtline = new DataTable();
                            dtline = DHuy.SELECT_SQL(
                                string.Format("Select TOP 1 RECEIPTLINENUMBER {0}.RECEIPTDETAIL ORDER BY RECEIPTLINENUMBER WHERE RECEIPTKEY='{1}'", Warehouse, receiptkey));

                            if (dtline.Rows.Count > 0)
                            {
                                // lấy ra số line từ bảng dtline
                                receipline = int.Parse(dtline.Rows[0][0].ToString());
                                receipline++;
                            }
                            string sku = DBase.StringReturn(rd["Item_Code"]);
                            DataTable dtsku = new DataTable();
                            dtsku = DHuy.SELECT_SQL(string.Format(
                                "SELECT * FROM {0}.SKU WHERE WHSEID='{0}' AND STORERKEY='" + storerkey +
                                "' AND SKU='" + sku + "'", Warehouse));
                            if (dtsku.Rows.Count > 0)
                            {
                                DataTable dt_detail =
                                DHuy.SELECT_NEWROW_FORSCHEMA(string.Format("{0}.RECEIPTDETAIL", Warehouse));
                                string packkey = DBase.StringReturn(dtsku.Rows[0]["PACKKEY"]);
                                DataTable dtpack = new DataTable();
                                dtpack = DHuy.SELECT_SQL(string.Format(
                                    "SELECT * FROM {0}.PACK WHERE WHSEID='{0}' AND PACKKEY='" + packkey + "'",
                                    Warehouse));
                                dt_detail.Rows[0]["WHSEID"] = Warehouse;
                                dt_detail.Rows[0]["RECEIPTKEY"] = receiptkey;
                                dt_detail.Rows[0]["RECEIPTLINENUMBER"] = receipline;
                                dt_detail.Rows[0]["EXTERNRECEIPTKEY"] = "";
                                dt_detail.Rows[0]["EXTERNLINENO"] = "WMS" + receipline;
                                dt_detail.Rows[0]["SUBLINENUMBER"] = "";
                                dt_detail.Rows[0]["POKEY"] = "";
                                dt_detail.Rows[0]["STORERKEY"] = storerkey;
                                dt_detail.Rows[0]["STATUS"] = "0";
                                dt_detail.Rows[0]["TARIFFKEY"] = "XXXXXXXXXX";
                                dt_detail.Rows[0]["SKU"] = sku;
                                dt_detail.Rows[0]["ALTSKU"] = "";
                                dt_detail.Rows[0]["ID"] = "";
                                dt_detail.Rows[0]["DATERECEIVED"] = DateTime.Now;
                                dt_detail.Rows[0]["QTYEXPECTED"] = DBase.IntReturn(rd["Quantity"]);
                                dt_detail.Rows[0]["QTYADJUSTED"] = 0;
                                dt_detail.Rows[0]["QTYRECEIVED"] = 0;

                                dt_detail.Rows[0]["UOM"] = dtpack.Rows[0]["PACKUOM3"];
                                dt_detail.Rows[0]["PACKKEY"] = packkey;

                                dt_detail.Rows[0]["TOLOC"] = "STAGE"; //DBase.StringReturn(r["Loc"]);
                                dt_detail.Rows[0]["TOLOT"] = "";
                                dt_detail.Rows[0]["TOID"] = receiptkey + receipline;
                                dt_detail.Rows[0]["CONDITIONCODE"] = "OK";

                                dt_detail.Rows[0]["LOTTABLE01"] = DBase.IntReturn(rd["Group_Id"]);
                                dt_detail.Rows[0]["LOTTABLE02"] = DBase.IntReturn(rd["Order_Line_Id"]);
                                dt_detail.Rows[0]["LOTTABLE03"] = DBase.IntReturn(rd["Order_Line_Detail_Id"]);
                                // dt_detail.Rows[0]["LOTTABLE04"] = "";
                                //dt_detail.Rows[0]["LOTTABLE05"] = "";
                                dt_detail.Rows[0]["LOTTABLE06"] = "";
                                dt_detail.Rows[0]["LOTTABLE07"] = DBase.StringReturn(rd["REFERENCE_NUMBER"]);
                                dt_detail.Rows[0]["LOTTABLE08"] = DBase.StringReturn(rd["Ship_From_Sub_Inventory"]);
                                dt_detail.Rows[0]["LOTTABLE09"] = "";
                                dt_detail.Rows[0]["LOTTABLE10"] = receiptkey + receipline;
                                dt_detail.Rows[0]["CASECNT"] = 0;
                                dt_detail.Rows[0]["INNERPACK"] = 0;
                                dt_detail.Rows[0]["PALLET"] = 0;
                                dt_detail.Rows[0]["CUBE"] = 0;
                                dt_detail.Rows[0]["GROSSWGT"] = 0;
                                dt_detail.Rows[0]["NETWGT"] = 0;
                                dt_detail.Rows[0]["OTHERUNIT1"] = 0;
                                dt_detail.Rows[0]["OTHERUNIT2"] = 0;
                                dt_detail.Rows[0]["UNITPRICE"] = 0;
                                dt_detail.Rows[0]["EXTENDEDPRICE"] = 0;
                                dt_detail.Rows[0]["PACKINGSLIPQTY"] = 0;

                                dt_detail.Rows[0]["FORTE_FLAG"] = "I";
                                dt_detail.Rows[0]["EFFECTIVEDATE"] = DateTime.Now;
                                dt_detail.Rows[0]["TYPE"] = 1;

                                dt_detail.Rows[0]["QTYREJECTED"] = 0;
                                dt_detail.Rows[0]["QCREQUIRED"] = 0;
                                dt_detail.Rows[0]["QCSTATUS"] = "N";
                                dt_detail.Rows[0]["QCAUTOADJUST"] = 0;

                                dt_detail.Rows[0]["MatchLottable"] = 0;

                                dt_detail.Rows[0]["ADDDATE"] = DateTime.Now;
                                dt_detail.Rows[0]["ADDWHO"] = "DCCanTho";
                                dt_detail.Rows[0]["EDITDATE"] = DateTime.Now;
                                dt_detail.Rows[0]["EDITWHO"] = "DCCanTho";
                                if (receiptkey != "")
                                {
                                    int rs = 0;
                                    rs = DHuy.INSERT_IDENTITY(string.Format("{0}.RECEIPTDETAIL", Warehouse),
                                        dt_detail);
                                    if (rs > 0)
                                    {
                                        totalinsert++;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Không có dữ liệu");
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("Không tạo được receipt");
                                }
                            }
                        }

                    }
                }
            }

        }

        private void btnChiaXe_Click(object sender, EventArgs e)
        {

        }
    }
}
