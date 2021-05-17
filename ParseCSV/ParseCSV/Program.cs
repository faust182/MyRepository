using System;
using static ParseCSV.Helper;

namespace ParseCSV
{
    class Program
    {
        static void Main(string[] args)
        {

            var input = GetInputData();
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
            var newMonth = new Meter(input.pathInputFile, input.month, input.year);
            if (!string.IsNullOrEmpty(input.pathOutputFile)) newMonth.pathForOutFile = input.pathOutputFile;
            newMonth.CreateOutputFile();
        }
    }
}
