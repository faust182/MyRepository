using System;
using System.Collections.Generic;
using System.IO;
using static ParseCSV.Constants;
using System.Text;

namespace ParseCSV
{
    static class Helper
    {
        // перобазует входящую строку в номер месяца
        public static int GetMonthNumber(string month)
        {
            switch (month)
            {
                case "1":
                case "январь":
                    return 1;
                case "2":
                case "февраль":
                    return 2;
                case "3":
                case "март":
                    return 3;
                case "4":
                case "апрель":
                    return 4;
                case "5":
                case "май":
                    return 5;
                case "6":
                case "июнь":
                    return 6;
                case "7":
                case "июль":
                    return 7;
                case "8":
                case "август":
                    return 8;
                case "9":
                case "сентябрь":
                    return 9;
                case "10":
                case "октябрь":
                    return 10;
                case "11":
                case "ноябрь":
                    return 11;
                case "12":
                case "декабрь":
                    return 12;
                default: Console.WriteLine("Неверно указан месяц"); 
                    return 0;
            }
        }

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
            var output = new InputData();
            Console.WriteLine(@"Введите полный путь до файла с данными (пример: d:\Program Files\...\Example meters.CSV)");
            output.pathInputFile = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine(@"Введите полный путь до файла в который будут помещены данные (пример: d:\Program Files\...\Result.CSV). Если путь не будет указан, то файл ""output.CSV"" с результатами будет находиться по директории запуска исполняемого файла");
            output.pathOutputFile = Console.ReadLine();
            Console.WriteLine();
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV

            Console.WriteLine("Введите год (допускается введение тоько полного значения):");
            output.year = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите название месяца (кириллицей) или его порядковый номер:");
            output.month = Console.ReadLine();

            return output;

        }
        public static double GetTimeMinuteInterval(DateTime startTime, DateTime endTime)
        {
            return (endTime - startTime).TotalMinutes;
        }
        public static double GetMaxInRange(List<double> inputList, Range range)
        {
            double val = int.MinValue;
            for (int i = range.Start; i < range.End + 1; i++)
            {
                if (inputList[i] > val) val = inputList[i];
            }
            return val;
        }
        public static double GetMinInRange(List<double> inputList, Range range)
        {
            double val = int.MaxValue;
            for (int i = range.Start; i < range.End + 1; i++)
            {
                if (inputList[i] < val) val = inputList[i];
            }
            return val;
        }
        public static List<string> ReadeCsv(string path)
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
            }
            return collector;
        }
        public static Range GetRowsRangeByMonthOfYear(Table inputTable, int month, int year)
        {
            int currentMonth = 0;
            int currentYear = 0;
            var outputRange = new Range {Start = int.MinValue, End = int.MinValue };
            for (int i = 0; i < inputTable.columnDate.Count; i++)
            {
                currentMonth = inputTable.columnDate[i].Month;
                currentYear = inputTable.columnDate[i].Year;
                if (currentMonth == month && currentYear == year && outputRange.Start == int.MinValue)
                {
                    outputRange.Start = i;
                }
                else if (outputRange.Start != int.MinValue && currentMonth != month && inputTable.columnDate[i - 1].Month == month)
                {
                    outputRange.End = i - 1;
                    break;
                }
                else if (outputRange.Start != int.MinValue)
                {
                    outputRange.End = i;
                }
            }
            if (outputRange.Start == int.MinValue)
            {
                Console.WriteLine("Неверно введена дата (месяц и/или год)");
            }

            return outputRange;
        }
        public static Table ParseCsv(List<string> inputList)
        {
            var output = new Table();
            var resultOfParsingDate = new DateTime();
            foreach (var line in inputList)
            {
                var tempArray = line.Split(';');
                if (DateTime.TryParse(tempArray[0], out resultOfParsingDate))
                {
                    output.columnDate.Add(resultOfParsingDate);
                    output.columnStartTime.Add(DateTime.Parse(tempArray[1]));
                    if (tempArray[2] == WrongTimeFormatMidnight)
                    {
                        output.columnEndTime.Add(DateTime.Parse(RightTimeFormatMidnight).AddDays(1));
                    }
                    else
                    {
                        output.columnEndTime.Add(DateTime.Parse(tempArray[2]));
                    }
                    output.columnActivePower.Add(double.Parse(tempArray[3]));
                    output.columnReactivePower.Add(double.Parse(tempArray[17]));
                }
            }
            
            return output;
        }
    }
}
