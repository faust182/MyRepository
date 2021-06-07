using System;
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
            bool isPathInputFileCorrect = false;
            bool isPathOutputFileCorrect = false;
            bool isYearCorrect = false;
            string pathOutputFile;
            int year;
            int month;
            var output = new InputData();
            Console.WriteLine(@"Введите полный путь до файла с данными (пример: d:\Program Files\...\Example meters.CSV)");
            Console.WriteLine();
            do
            {
                string input = Console.ReadLine();
                if (Validator.IsPathInputFileCorret(input))
                {
                    output.PathInputFile = input;
                    isPathInputFileCorrect = true;
                }
                else
                {
                    Console.WriteLine("Неверно указан путь до файла-источника");
                    Console.WriteLine("Повторите попытку");
                    Console.WriteLine();
                }
            } 
            while (!isPathInputFileCorrect);

            Console.WriteLine();
            Console.WriteLine(@"Введите полный путь до файла в который будут помещены результаты (пример: d:\Program Files\...\Result.CSV). Если путь не будет указан, то файл ""output.CSV"" с результатами будет находиться по директории запуска исполняемого файла");
            Console.WriteLine();
            do
            {
                pathOutputFile = Console.ReadLine();
                var validation = Validator.IsPathOutputFileCorret(pathOutputFile);
                if (validation.isNameCorrect && validation.isPathCorrect)
                {
                    output.PathOutputFile = pathOutputFile;
                    isPathOutputFileCorrect = true;
                }
                else
                {
                    if (validation.isNameCorrect == false)
                    {
                        Console.WriteLine("Неверно указано имя файла (используются недопустимые символы или неверно указано расширение)"); 
                    }

                    if (validation.isPathCorrect == false)
                    {
                        Console.WriteLine("Неверно указана директория");
                    }

                    Console.WriteLine("Повторите попытку");
                    Console.WriteLine();
                }
            }
            while (!isPathOutputFileCorrect);

            Console.WriteLine();
            Console.WriteLine("Введите год (допускается ввод тоько полного значения):");
            do
            {
                isYearCorrect = int.TryParse(Console.ReadLine(), out year);
                if (isYearCorrect)
                {
                    output.Year = year;
                }
                else
                {
                    Console.WriteLine("Неверно указан год");
                    Console.WriteLine("Повторите попытку");
                    Console.WriteLine();
                }
            }
            while (!isYearCorrect);

            Console.WriteLine();
            Console.WriteLine("Введите название месяца (кириллицей) или его порядковый номер:");
            do
            {        
                string inputMonth = Console.ReadLine();
                month = Validator.GetMonthNumber(inputMonth);
                if (month != 0) output.Month = inputMonth;
                else
                {
                    Console.WriteLine("Неверно указан месяц");
                    Console.WriteLine("Повторите попытку");
                    Console.WriteLine();
                }
            }
            while (month == 0);
            
            Console.WriteLine();
            return output;
        }

        // получает временной интервал (в минутах) за который было усреднено значение мощностей
        public static double GetTimeMinuteInterval(DateTime startTime, DateTime endTime)
        {
            return (endTime - startTime).TotalMinutes;
        }

        // получает максимальное значение в списке в указанном диапазоне
        public static double GetMaxInRange(MyTable inputList, Range range, string typeOFPower = "ActivePower")
        {
            double val = int.MinValue;
            if (typeOFPower == "ActivePower")
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
        public static double GetMinInRange(MyTable inputList, Range range, string typeOFPower = "ActivePower")
        {
            double val = int.MaxValue;
            if (typeOFPower == "ActivePower")
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
            var output = new MyTable();
            var resultOfParsingDate = new DateTime();
            int rowCounter = 0;
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
                    output[rowCounter] = row;
                    rowCounter++;
                }
            }

            return output;
        }
    }
}
