using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ParseCSV
{
    class Helper
    {
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
        // перобазует входящую строку в номер месяца
        public static void GetMinAndMax(double val, ref double min, ref double max)
        {
            if (val > max)
            {
                max = val;
            }
            if (val < min)
            {
                min = val;
            }
        }
        // присваевает соответствующим переменным значение val если оно оказалось больше или меньше присвоенных  
        public static (int start, int end) GetRowsRangeByMonth(string[,] array, int month, int year)
        {
            var out_tuple = (s: -1, f: -1);
            var cur_month = new int();
            var cur_year = new int();
            //int s = -1;
            //int f = -1;
            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    cur_month = DateTime.Parse(array[0, i]).Month;
                    cur_year = DateTime.Parse(array[0, i]).Year;
                    if (cur_month == month && cur_year == year && out_tuple.s == -1)
                    {
                        out_tuple.s = i;
                    }
                    else if (cur_month != month && DateTime.Parse(array[0, i - 1]).Month == month)
                    {
                        out_tuple.f = i - 1;
                        break;
                    }
                    else
                    {
                        out_tuple.f = i;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            //Console.WriteLine(out_tuple.s);
            //Console.WriteLine(out_tuple.f);
            if (out_tuple.s == -1)
            {
                Console.WriteLine("Неверно введена дата (месяц и/или год)");
            }
            return out_tuple;
        }
        // возвращает два значения- с какой и по какою "строку" в двухмерном массиве находятся данные соответствующие запрошенному периоду (месяцу)
        public static double GetMinuteInterval(string StartTime, string EndTime)
        {
            TimeSpan interval = new TimeSpan();
            if (EndTime == "24:00:00")
            {
                interval = DateTime.Today.AddDays(1) - DateTime.Parse(StartTime);
            }
            else
            {
                interval = DateTime.Parse(EndTime) - DateTime.Parse(StartTime);
            }
            return interval.TotalMinutes;
        }
        // возвращает количество минут на котором производилось усреднение значения в исходном файле
        public static string[,] GetArrayFromCsvFile(string path)
        {
            StringBuilder input = new StringBuilder();
            var number_rows = new int();
            var number_collumns = new int();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {

                    string line;//наверно, нехорошо использовать тут string, но в примере на metanit так...
                    while ((line = sr.ReadLine()) != null)
                    {
                        input.Append(line + '\n');
                        //number_rows++;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Произошла ошибка при чтении файла. Проверьите введенный путь до файла-источника или проверьте, чтобы файл не истользовался другим приложением");
            }
            string[] array = input.ToString().Split('\n');
            number_rows = array.Length;
            number_collumns = array[0].Split(';').Length;
            string[] temp_array = new string[number_collumns];
            string[,] output = new string[number_collumns, number_rows];
            for (int i = 0; i < number_rows - 1; i++)
            {
                temp_array = array[i].Split(';');
                for (int j = 0; j < number_collumns - 1; j++)
                {
                    output[j, i] = temp_array[j];
                }
            }
            return output;
        }
        // формирует двухмерный массив данных размерностью [количество колонок, количество строк] из исходной таблицы (файла)
        public static void MakeCsvFile(string path, string[] collunms_name, double[] val_array)
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
        // записывает CVS файл, по указанной директории, содержащий всего две строки: первая содержит названия колонок, вторая- соответствующие названиям колонок значения
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
        // реализует консольный ввод требуемых значений (путь до файла-источника, год, месяц, путь до файла с результатами)
    }
}
