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
            item1.CreateOutputFile();

            // f:\Anton\Work\C#\TestArea\Example meters.CSV
        }
    }
}
