using System;

namespace ParseCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            var item1 = new Meter();
            item1.GetData();
            item1.CalculateValues();
            item1.CreateOutputFile();
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
        }
    }
}
