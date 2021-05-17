using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ParseCSV
{
    static class Helper
    {
        // перобазует входящую строку в номер месяца
        public static int GetMonthNumber(string month)
        {
            switch (month)
            {
                case "январь": case "1": return 1;
                case "февраль": case "2": return 2;
                case "март": case "3": return 3;
                case "апрель": case "4": return 4;
                case "май": case "5": return 5;
                case "июнь": case "6": return 6;
                case "июль": case "7": return 7;
                case "август": case "8": return 8;
                case "сентябрь": case "9": return 9;
                case "октябрь": case "10": return 10;
                case "ноябрь": case "11": return 11;
                case "декабрь": case "12": return 12;
                default: Console.WriteLine("Неверно указан месяц"); return 0;
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
                Console.WriteLine("Запись выполнена");
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
            output.month = Console.ReadLine().ToLower();

            return output;

        }
        public static double GetTimeMinuteInterval(DateTime startTime, DateTime endTime)
        {
            return (endTime - startTime).TotalMinutes;
        }
        public static double GetMaxInRange(List<double> inputList, Range range)
        {
            double val = int.MinValue; 
            for (int i = range.start; i < range.end + 1; i++)
            {             
                if (inputList[i] > val) val = inputList[i]; 
            }
            return val;
        }
        public static double GetMinInRange(List<double> inputList, Range range)
        {
            double val = int.MaxValue;
            for (int i = range.start; i < range.end + 1; i++)
            { 
                if (inputList[i] < val) val = inputList[i];
            }
            return val;
        }
        public static Range GetRowsRangeByMonthOfYear(Table inputTable, int month, int year)
        {
            var outputRange = new Range {start = int.MinValue, end = int.MinValue };
            int currentMonth = 0;
            int currentYear = 0;
            for (int i = 0; i < inputTable.columnDate.Count; i++)
            {
                currentMonth = inputTable.columnDate[i].Month;
                currentYear = inputTable.columnDate[i].Year;
                if (currentMonth == month && currentYear == year && outputRange.start == int.MinValue)
                {
                    outputRange.start = i;
                }
                else if (outputRange.start != int.MinValue && currentMonth != month && inputTable.columnDate[i - 1].Month == month)
                {
                    outputRange.end = i - 1;
                    break;
                }
                else if (outputRange.start != int.MinValue)
                {
                    outputRange.end = i;
                }
            }
            if (outputRange.start == int.MinValue)
            {
                Console.WriteLine("Неверно введена дата (месяц и/или год)");
            }

            return outputRange;
        }
 
        public static Table Parser(string path)
        {
            var output = new Table();
            var resultOfParsingDate = new DateTime();
            try
            {
                using (StreamReader tempString = new StreamReader(path))
                {

                    string line;
                    while ((line = tempString.ReadLine()) != null)
                    {
                        var tempListForValues = new List<string>(line.Split(Constants.separatorForCsvString));
                        if (DateTime.TryParse(tempListForValues[0], out resultOfParsingDate))
                        {
                            output.columnDate.Add(resultOfParsingDate);
                            output.columnStartTime.Add(DateTime.Parse(tempListForValues[1]));
                            if (tempListForValues[2] == Constants.wrongTimeFormatMidnight)
                            {
                                output.columnEndTime.Add(DateTime.Parse(Constants.rightTimeFormatMidnight).AddDays(1));
                            }
                            else
                            {
                                output.columnEndTime.Add(DateTime.Parse(tempListForValues[2]));
                            }
                            output.columnActivePower.Add(double.Parse(tempListForValues[3]));
                            output.columnReactivePower.Add(double.Parse(tempListForValues[17]));
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Произошла ошибка при чтении файла. Проверьите введенный путь до файла-источника или проверьте чтобы файл не истользовался другим приложением");
            }
            return output;
        }
    }
}
