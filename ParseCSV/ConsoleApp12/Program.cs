using System;
using System.IO;
using System.Text;

namespace ConsoleApp12
{
    class Program
    {
        static void Main(string[] args)
        {
            static (int hour, int minute, int second) WhatIsTimeInterval(string StartTime, string EndTime)
            {
                var result = (hour: 0, minute: 0, second: 0);
                string[] start_array_val = StartTime.Split(':');
                string[] end_array_val = EndTime.Split(':');
                result.hour = (int.Parse(end_array_val[0]) - int.Parse(start_array_val[0]));
                result.minute = int.Parse(end_array_val[1]) - int.Parse(start_array_val[1]);
                if (result.minute < 0)
                {
                    result.hour -= 1;
                    result.minute = 60 - int.Parse(start_array_val[1]);
                }
                result.second = int.Parse(end_array_val[2]) - int.Parse(start_array_val[2]);
                if (result.second < 0)
                {
                    result.minute -= 1;
                    result.second = 60 - int.Parse(start_array_val[2]);
                }
                return result;
            }
            var tuple = WhatIsTimeInterval("17:34:10", "18:00:00");
            Console.Write(tuple.hour.ToString() + ':');
            Console.Write(tuple.minute.ToString() + ':');
            Console.Write(tuple.second.ToString());
        }
    }
}
