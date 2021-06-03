using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    class TableConstructor
    {
        public class Row
        {
            public DateTime Date { get; set; }

            public DateTime StartTime { get; set; }

            public DateTime EndTime { get; set; }

            public double ActivePower { get; set; }

            public double ReactivePower { get; set; }
        }

        public class Table
        {
            List<Row> table;

            public Table()
            {
                table = new List<Row>();
            }

            public Row this[int index]
            {
                get
                {
                    return table[index];
                }

                set
                {
                    table.Insert(index, value);
                }
            }
        }
    }
}
