﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using static ParseCSV.Constants;

namespace ParseCSV
{
    static class Helper
    {
        // записывает CVS файл, по указанной директории, содержащий всего две строки: первая содержит названия колонок, вторая- соответствующие названиям колонок значения
        public static void CreateCsvFile(string path, string[] collunms_name, double[] val_array)
        {
            StringBuilder str = new StringBuilder();
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    foreach (var item in collunms_name)
                    {
                        str.Append(item + ";");
                    }

                    str.Remove(str.Length - 1, 1);
                    sw.WriteLine(str.ToString());
                    str.Clear();
                    foreach (var item in val_array)
                    {
                        str.Append(item.ToString() + ";");
                    }

                    str.Remove(str.Length - 1, 1);
                    sw.WriteLine(str.ToString());
                }

                Console.WriteLine("Создание файла выполнено");
            }
            catch (Exception e)
            {
                Console.WriteLine("Создание файла не выполнено");
                Console.WriteLine(e.Message);
            }
        }

        // реализует консольный ввод требуемых значений (путь до файла-источника, год, месяц, путь до файла с результатами)
        public static InputData GetInputData()
        {
            var input = new InputData();
            input.GetPathInputFile();
            
            do
            {
                input.GetPathOutputFile();
                if (input.PathInputFile == input.PathOutputFile)
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "!!!Внимание!!!\n" +
                        "Имя файла с результатами вычислений совпадает с именем файла-источника, " +
                        "во избежании перезаписи введите иное имя");
                }
            }
            while (input.PathInputFile == input.PathOutputFile);
    
            input.GetYear();
            input.GetMonth();
            Console.WriteLine();
            return input;
        }

        // получает временной интервал (в минутах) за который было усреднено значение мощностей
        public static double GetTimeMinuteInterval(DateTime startTime, DateTime endTime)
        {
            return (endTime - startTime).TotalMinutes;
        }

        // получает максимальное значение в списке в указанном диапазоне
        public static double GetMaxInRange(MyTable inputList, Range range, TypeOfPower type)
        {
            double val = int.MinValue;
            if (type == 0)
            {
                for (int i = range.Start; i < range.End + 1; i++)
                {
                    if (inputList[i].ActivePower > val) val = inputList[i].ActivePower;
                }
            }
            else
            {
                for (int i = range.Start; i < range.End + 1; i++)
                {
                    if (inputList[i].ReactivePower > val) val = inputList[i].ReactivePower;
                }
            }

            return val;
        }

        // получает минимальное значение в списке в указанном диапазоне
        public static double GetMinInRange(MyTable inputList, Range range, TypeOfPower type)
        {
            double val = int.MaxValue;
            if (type == 0)
            {
                for (int i = range.Start; i < range.End + 1; i++)
                {
                    if (inputList[i].ActivePower < val) val = inputList[i].ActivePower;
                }
            }
            else
            {
                for (int i = range.Start; i < range.End + 1; i++)
                {
                    if (inputList[i].ReactivePower < val) val = inputList[i].ReactivePower;
                }
            }

            return val;
        }

        public static List<string> ReadCsv(string path)
        {
            var collector = new List<string>();
            try
            {
                using (StreamReader tempString = new StreamReader(path))
                {
                    string line;

                    while ((line = tempString.ReadLine()) != null)
                    {
                        collector.Add(line);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Произошла ошибка при чтении файла. Проверьите введенный путь до файла-источника или проверьте, чтобы файл не истользовался другим приложением");
                Console.ReadLine();
            }

            return collector;
        }

        public static Range GetRowsRangeByMonthOfYear(MyTable inputTable, int month, int year)
        {
            int currentMonth = 0;
            int currentYear = 0;
            int counter = 0;
            var outputRange = new Range { Start = int.MinValue, End = int.MinValue };
            for (int i = 0; i < inputTable.Table.Count; i++)
            {
                currentMonth = inputTable[i].Date.Month;
                currentYear = inputTable[i].Date.Year;
                if (currentMonth == month && currentYear == year && outputRange.Start == int.MinValue)
                {
                    outputRange.Start = counter;
                }
                else if (outputRange.Start != int.MinValue && currentMonth != month && inputTable[i - 1].Date.Month == month)
                {
                    outputRange.End = counter - 1;
                    break;
                }
                else if (outputRange.Start != int.MinValue)
                {
                    outputRange.End = counter;
                }
                
                counter++;
            }

            if (outputRange.Start == int.MinValue)
            {
                Console.WriteLine("Неверно введена дата (месяц и/или год)");
            }

            return outputRange;
        }

        public static MyTable ParseCsv(List<string> inputList)
        {
            var rows = new List<Row>();
            var resultOfParsingDate = new DateTime();
            foreach (var line in inputList)
            {
                var tempArray = line.Split(';');
                var row = new Row();
                if (DateTime.TryParse(tempArray[0], out resultOfParsingDate))
                {
                    row.Date = resultOfParsingDate;
                    row.StartTime = DateTime.Parse(tempArray[1]);
                    if (tempArray[2] == WrongTimeFormatMidnight)
                    {
                        row.EndTime = DateTime.Parse(RightTimeFormatMidnight).AddDays(1);
                    }
                    else
                    {
                        row.EndTime = DateTime.Parse(tempArray[2]);
                    }

                    row.ActivePower = double.Parse(tempArray[3]);
                    row.ReactivePower = double.Parse(tempArray[17]);
                    rows.Add(row);
                }
            }

            return new MyTable(rows);
        }
    }
}
