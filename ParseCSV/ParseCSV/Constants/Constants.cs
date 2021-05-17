using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    static public class Constants
    {
        public const char separatorForCsvString = ';';
        public const string wrongTimeFormatMidnight = "24:00:00";
        public const string rightTimeFormatMidnight = "00:00:00";
        public const string defaultNameOutputFile = "\\output.CSV";
    }
}
