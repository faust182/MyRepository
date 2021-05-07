using System;
using System.IO;
using System.Text;

namespace ParseCSV
{
    class Program
    {
        static void Main(string[] args)
        {

            //string.IsNullOrEmpty();
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
            /*var input = Helper.GetInputData();
            var a = new FirstIntegrator(input.path_in, input.month, input.year);
            if (input.path_out != "") a.path_out_f = input.path_out;
            a.CookFile();*/
            var path = @"d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV";
            var newMonth = new SecondIntegrator(path, "3", 2017);
            newMonth.CookOutputFile();
        }
    }
}
