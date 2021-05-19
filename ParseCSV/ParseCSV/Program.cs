using System;

namespace ParseCSV
{
    class Program
    {
        static void Main(string[] args)
        {

            var input = Helper.GetInputData();
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
            var month = new Meter(input.pathInputFile, input.month, input.year);
            if (!string.IsNullOrEmpty(input.pathOutputFile)) month.pathForOutFile = input.pathOutputFile;
            month.GetTableData();
            if (month.flagSuccessReadFile) month.CreateOutputFile();
        }
    }
}
