using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ParseCSV
{
    class Helper
    {
        // перобазует входящую строку в номер месяца
        public static int GetMonthNumber(string month)
        {
            switch (month)
            {
                case "январь":
                case "1":
                    return 1;
                case "февраль":
                case "2":
                    return 2;
                case "март":
                case "3":
                    return 3;
                case "апрель":
                case "4":
                    return 4;
                case "май":
                case "5":
                    return 5;
                case "июнь":
                case "6":
                    return 6;
                case "июль":
                case "7":
                    return 7;
                case "август":
                case "8":
                    return 8;
                case "сентябрь":
                case "9":
                    return 9;
                case "октябрь":
                case "10":
                    return 10;
                case "ноябрь":
                case "11":
                    return 11;
                case "декабрь":
                case "12":
                    return 12;
                default:
                    Console.WriteLine("Неверно указан месяц");
                    return 0;
            }
        }
        // записывает CVS файл, по указанной директории, содержащий всего две строки: первая содержит названия колонок, вторая- соответствующие названиям колонок значения
        public static void CriateCsvFile(string path, string[] collunms_name, double[] val_array)
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
        public static (string path_in, string path_out, string month, int year) GetInputData()
        {
            var out_tuple = (pi: "", po: "", m: "", y: 0);
            Console.WriteLine(@"Введите полный путь до файла с данными (пример: d:\Program Files\...\Example meters.CSV)");
            out_tuple.pi = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine(@"Введите полный путь до файла в который будут помещены данные (пример: d:\Program Files\...\Result.CSV). Если путь не будет указан, то файл ""output.CSV"" с результатами будет находиться по директории запуска исполняемого файла");
            out_tuple.po = Console.ReadLine();
            Console.WriteLine();
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV

            Console.WriteLine("Введите год (допускается введение тоько полного значения):");
            out_tuple.y = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите название месяца (кириллицей) или его порядковый номер:");
            out_tuple.m = Console.ReadLine();

            return out_tuple;

        }
        public static double GetTimeMinuteInterval(DateTime startTime, DateTime endTime)
        {
            double outputMinute = 0;
            outputMinute = (endTime - startTime).TotalMinutes;
            return outputMinute;
        }
        public static double GetMaxInRange(List<double> inputList, Range range)
        {
            double val = 0;
            bool flagFirsIteration = true;
            for (int i = range.Start; i < range.End + 1; i++)
            {
                if (flagFirsIteration)
                {
                    val = inputList[i];
                    flagFirsIteration = false;
                }
                else
                {
                    if (inputList[i] > val) val = inputList[i];
                }
            }
            return val;
        }
        public static double GetMinInRange(List<double> inputList, Range range)
        {
            double val = 0;
            bool flagFirsIteration = true;
            for (int i = range.Start; i < range.End + 1; i++)
            {
                if (flagFirsIteration)
                {
                    val = inputList[i];
                    flagFirsIteration = false;
                }
                else
                {
                    if (inputList[i] < val) val = inputList[i];
                }
            }
            return val;
        }
        public static Range GetRowsRangeByMonthOfYear(Table inputTable, int month, int year)
        {
            var outputRange = new Range {Start = -1, End = -1 };
            var currentMonth = new int();
            var currentYear = new int();
            for (int i = 0; i < inputTable.ColumnDate.Count; i++)
            {
                currentMonth = inputTable.ColumnDate[i].Month;
                currentYear = inputTable.ColumnDate[i].Year;
                if (currentMonth == month && currentYear == year && outputRange.Start == -1)
                {
                    outputRange.Start = i;
                }
                else if (outputRange.Start != -1 && currentMonth != month && inputTable.ColumnDate[i - 1].Month == month)
                {
                    outputRange.End = i - 1;
                    break;
                }
                else
                {
                    outputRange.End = i;
                }
            }
            //Console.WriteLine(out_tuple.s);
            //Console.WriteLine(out_tuple.f);
            if (outputRange.Start == -1)
            {
                Console.WriteLine("Неверно введена дата (месяц и/или год)");
            }

            return outputRange;
        }
        // формирует структуру данных типа Table из исходной таблицы (файла)
        public static Table GetStructFromInputCsvFile(string path)
        {
            var unitedStringFromFile = new StringBuilder();
            var wrongTimeFormatMidnight = "24:00:00";
            var rightTimeFormatMidnight = "00:00:00";
            try
            {
                using (StreamReader tempString = new StreamReader(path))
                {

                    string line;
                    while ((line = tempString.ReadLine()) != null)
                    {
                        unitedStringFromFile.Append(line + '\n');
                    }
                }
            }
            catch
            {
                Console.WriteLine("Произошла ошибка при чтении файла. Проверьите введенный путь до файла-источника или проверьте, чтобы файл не истользовался другим приложением");
            }
            var outputStruct = new Table();
            var tempListForRows = new List<string>(unitedStringFromFile.ToString().Split('\n'));
            foreach (var i in tempListForRows)
            {
                var tempListForValues = new List<string>(i.Split(';'));
                try
                {
                    outputStruct.ColumnDate.Add(DateTime.Parse(tempListForValues[0]));
                    outputStruct.ColumnStartTime.Add(DateTime.Parse(tempListForValues[1]));
                    if (tempListForValues[2] == wrongTimeFormatMidnight)
                    {
                        outputStruct.ColumnEndTime.Add(DateTime.Parse(rightTimeFormatMidnight).AddDays(1));
                    }
                    else
                    {
                        outputStruct.ColumnEndTime.Add(DateTime.Parse(tempListForValues[2]));
                    }
                    outputStruct.ColumnActivePower.Add(double.Parse(tempListForValues[3]));
                    outputStruct.ColumnReactivePower.Add(double.Parse(tempListForValues[17]));
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return outputStruct;
        }

    }
}
