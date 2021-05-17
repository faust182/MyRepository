using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    static public class Constants
    {
        public static int defaultValue = int.MinValue;
        public static char separatorForStrings = '\n';
        public static char separatorForCsvString = ';';
        public static string wrongTimeFormatMidnight = "24:00:00";
        public static string rightTimeFormatMidnight = "00:00:00";
        public static string defaultNameOutputFile = "\\output.CSV";
    }
}
