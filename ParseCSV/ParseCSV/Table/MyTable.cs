using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    class MyTable
    {
        public List<Row> Table;

        public MyTable()
        {
            Table = new List<Row>();
        }

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
