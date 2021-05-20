using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    class Table
    {
        public Table()
        {
            ColumnDate = new List<DateTime>();
            ColumnStartTime = new List<DateTime>();
            ColumnEndTime = new List<DateTime>();
            ColumnActivePower = new List<double>();
            ColumnReactivePower = new List<double>();
        }

        public List<DateTime> ColumnDate { get; set; }

        public List<DateTime> ColumnStartTime { get; set; }

        public List<DateTime> ColumnEndTime { get; set; }

        public List<double> ColumnActivePower { get; set; }

        public List<double> ColumnReactivePower { get; set; }
    }
}
