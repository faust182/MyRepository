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
            // it add for test git!!!
            var newMonth = new Integrator(input.path_in, input.month, input.year);
            if (!string.IsNullOrEmpty(input.path_out)) newMonth.pathForOutFile = input.path_out;
            newMonth.CreateOutputFile();
        }
    }
}
