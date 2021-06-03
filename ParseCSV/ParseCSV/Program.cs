using System;

namespace ParseCSV
{
    public class Program
    {
        static void Main(string[] args)
        {
            var item1 = new Meter();
            item1.GetData();
            item1.CalculateValues();
            //Console.WriteLine(item1.Table2[0].ActivePower);
            item1.CreateOutputFile();

            // d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
        }
    }
}
