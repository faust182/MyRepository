using System;
using System.IO;
using System.Text;

namespace ParseCSV
{
    class Program
    {
        static void Main(string[] args)
        {

            var input = Helper.GetInputData();
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
            var newMonth = new Integrator(input.PathInputFile, input.Month, input.Year);
            if (!string.IsNullOrEmpty(input.PathOutputFile)) newMonth.pathForOutFile = input.PathOutputFile;
            newMonth.CreateOutputFile();
        }
    }
}
