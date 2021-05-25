using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ParseCSV
{
    class Table
    {
        public Table()
        {
            DataTable simpleTable = new DataTable("Meter");
            DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            idColumn.Unique = true;
            idColumn.AllowDBNull = false;
            idColumn.AutoIncrement = true;
            idColumn.AutoIncrementSeed = 1;
            idColumn.AutoIncrementStep = 1;

            DataColumn dateColumn = new DataColumn("Date", Type.GetType("System.DataTime"));
            DataColumn startTimeColumn = new DataColumn("Start Time", Type.GetType("System.DataTime"));
            DataColumn endTimeColumn = new DataColumn("End Time", Type.GetType("System.DataTime"));
            DataColumn activePower = new DataColumn("Active Power", Type.GetType("System.Double"));
            DataColumn reactivePower = new DataColumn("Reactive Power", Type.GetType("System.Double"));

            simpleTable.Columns.Add(idColumn);
            simpleTable.Columns.Add(dateColumn);
            simpleTable.Columns.Add(startTimeColumn);
            simpleTable.Columns.Add(endTimeColumn);
            simpleTable.Columns.Add(activePower);
            simpleTable.Columns.Add(reactivePower);
            simpleTable.PrimaryKey = new DataColumn[] { simpleTable.Columns["Id"] };

            ColumnDate = new List<DateTime>();
            ColumnStartTime = new List<DateTime>();
            ColumnEndTime = new List<DateTime>();
            ColumnActivePower = new List<double>();
            ColumnReactivePower = new List<double>();
        }

        public DataTable SimpleTable { get; set; }

        public List<DateTime> ColumnDate { get; set; }

        public List<DateTime> ColumnStartTime { get; set; }

        public List<DateTime> ColumnEndTime { get; set; }

        public List<double> ColumnActivePower { get; set; }

        public List<double> ColumnReactivePower { get; set; }
    }
}
