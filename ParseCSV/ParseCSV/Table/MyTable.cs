using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    class MyTable
    {
        public MyTable(List<Row> rows)
        {
            Table = rows;
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
                if (index < Table.Count)
                {
                    Table.Insert(index, value);
                }
                else
                {
                    Table.Add(value);
                }
            }
        }
    }
}
