using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Transport
{
    class Result
    {
        protected int _RESULT = 0;
        protected string _MESSAGE = string.Empty;
        protected DataSet _DATASET = null;

        public Result()
        {
            this.RESULT = _RESULT;
            this.MESSAGE = _MESSAGE;
            this.DATASET = _DATASET;
       
        }

        public Result(int result, string message)
        {
            this.RESULT = result;
            this.MESSAGE = message;
        }

        public Result(int result, string message, DataSet dataset)
        {
            this.RESULT = result;
            this.MESSAGE = message;
            this.DATASET = dataset;
        }

        public int RESULT { get; set; }
        public string MESSAGE { get; set; }
        public DataSet DATASET { get; set; }
        public  DataTable DATATABLE { get; set; }
    }
}
