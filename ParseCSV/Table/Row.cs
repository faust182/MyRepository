using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    class Row
    {
        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public double ActivePower { get; set; }

        public double ReactivePower { get; set; }
    }
}
