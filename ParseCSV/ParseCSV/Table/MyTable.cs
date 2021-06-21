using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    class MyTable
    {
        public MyTable()
        {
            Table = new List<Row>();
        }

        public List<Row> Table { get; set; }

        public Row this[int index]
        {
            get
            {
                return Table[index];
            }

            set
            {
                Table.Insert(index, value);
            }
        }
    }
}
