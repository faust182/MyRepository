using System;
using System.IO;
using System.Text;

namespace ParseCSV
{
    class Program
    {
        static void Main(string[] args)
        {   
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
            var input = Helper.GetInputData();
            Integrator a = new Integrator(input.path_in, input.month, input.year);
            if (input.path_out != "") a.path_out_f = input.path_out;
            a.CookFile();
        }
    }
}
