using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciChartExport
{
    public static class Global
    {
        private static DataTable _globalVar;
        private static int _Ymehvar;
        private static int _NullValue;
        public static DataTable GlobalVar
        {
            get { return _globalVar; }
            set { _globalVar = value; }
        }
        public static int GlobalYmehvar
        {
            get { return _Ymehvar; }
            set { _Ymehvar = value; }
        }
        public static int GlobalNullValue
        {
            get { return _NullValue; }
            set { _NullValue = value; }
        }
    }
}
