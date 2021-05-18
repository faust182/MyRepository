using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    static public class Constants
    {
        public const char SeparatorForCsvString = ';';
        public const string WrongTimeFormatMidnight = "24:00:00";
        public const string RightTimeFormatMidnight = "00:00:00";
        public const string DefaultNameOutputFile = "\\output.CSV";
    }
}
