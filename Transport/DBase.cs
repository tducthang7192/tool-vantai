using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Globalization;

namespace Transport
{
    public class DBase
    {
        public static string StringReturn(object d) //return String from object
        {
            return d == null ? "" : d.ToString();
        }
        public static bool ShowQuestion(String QuestionString)
        {
            bool kq = false;

            if (MessageBox.Show(QuestionString, "Confirmed", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                kq = true;
            }

            return kq;

        }
        public static int IntReturn(object d)  //return number from object
        {
            return (isDouble(d) ? (int)double.Parse(d.ToString()) : 0);
        }
        public static bool isDouble(object number) //Check double
        {
            bool kq = true;
            try
            {
                Double.Parse(number.ToString());
            }
            catch (Exception)
            {
                kq = false;
            }

            return kq;
        }
        public static decimal DecimalReturn(object d) //return number from object
        {
            NumberFormatInfo nfi = (NumberFormatInfo)
                CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = "";
            return (isDouble(d) ? Decimal.Parse(d.ToString(), nfi) : 0);
        }


    }
}
