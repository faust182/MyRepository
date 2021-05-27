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
            SimpleTable = new DataTable("Meter");
            DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            idColumn.Unique = false;
            idColumn.AllowDBNull = true;
            idColumn.AutoIncrement = true;
            idColumn.AutoIncrementSeed = 1;
            idColumn.AutoIncrementStep = 1;

            DataColumn dateColumn = new DataColumn("Date", Type.GetType("System.DateTime"));
            DataColumn startTimeColumn = new DataColumn("Start Time", Type.GetType("System.DateTime"));
            DataColumn endTimeColumn = new DataColumn("End Time", Type.GetType("System.DateTime"));
            DataColumn activePowerColumn = new DataColumn("Active Power", Type.GetType("System.Double"));
            DataColumn reactivePowerColumn = new DataColumn("Reactive Power", Type.GetType("System.Double"));

            SimpleTable.Columns.Add(idColumn);
            SimpleTable.Columns.Add(dateColumn);
            SimpleTable.Columns.Add(startTimeColumn);
            SimpleTable.Columns.Add(endTimeColumn);
            SimpleTable.Columns.Add(activePowerColumn);
            SimpleTable.Columns.Add(reactivePowerColumn);
            SimpleTable.PrimaryKey = new DataColumn[] { SimpleTable.Columns["Id"] };

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
