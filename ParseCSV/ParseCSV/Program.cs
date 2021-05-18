using System;

namespace ParseCSV
{
    class Program
    {
        static void Main(string[] args)
        {

            var input = Helper.GetInputData();
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
            var newMonth = new Meter(input.pathInputFile, input.month, input.year);
            if (!string.IsNullOrEmpty(input.pathOutputFile)) newMonth.pathForOutFile = input.pathOutputFile;
            newMonth.CreateOutputFile();
        }
    }
}
