using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ParseCSV
{
    class Support
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
        public static void MinAndMaxFinder(double val, ref double min, ref double max)
        {
            if (val > max)
            {
                max = val;
            }
            if (val < min)
            {
                min = val;
            }
        } //название - глагол выходного параметраметра
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
                    }

                }
                catch (Exception)
                {

                    continue;
                }
            }
            return out_tuple;
        }
        public static double GetMinuteInterval(string StartTime, string EndTime)
        {
            TimeSpan interval = DateTime.Parse(EndTime) - DateTime.Parse(StartTime);
            return interval.TotalMinutes;
        }
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
                Console.WriteLine("Произошла ошибка при чтении файла");
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
        public static void MakeCsvFile(string path, string[] collunms_name, string[] val_array)
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
                        str.Append(item + ";");
                    }
                    str.Remove(str.Length - 1, 1);
                    sw.WriteLine(str.ToString());
                }
                Console.WriteLine("Запись выполнена");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /*public static (double sum_P, double sum_Q, double max_P, double min_P, double max_Q, double min_Q, double sq_P, double sq_Q, double sum_minutes, int sum_periods) Integrator //избегаем ref и out
            (int month, int year, int number_rows, string[,] data_array)
        {
            bool first_step_flag = true; //костыль, чтобы при первом проходе присвоить какое-то значение result.Pmin и result.Qmin,
                                         //иначе они остануться равны проинициализированным "0", т.к. в источнике все значения больше нуля
            var result = (P: 0.0, Q: 0.0, Pmax: 0.0, Pmin: 0.0, Qmax: 0.0, Qmin: 0.0, Psq: 0.0, Qsq: 0.0, minutes: 0.0, periods: 0);
            for (int i = 0; i < number_rows; i++)
            {
                DateTime current_date = DateTime.Parse(data_array[0, i]);
                if (current_date.Month == month && current_date.Year == year)
                {
                    var time_interval = Support.GetMinuteInterval(data_array[1, i], data_array[2, i]);
                    result.minutes += time_interval;
                    double cur_P = double.Parse(data_array[3, i]);
                    double cur_Q = double.Parse(data_array[17, i]);
                    if (first_step_flag) //продолжение костыля с first_step_flag, смотреть коммент выше
                    {
                        result.Pmin = cur_P;
                        result.Pmax = cur_P;
                        result.Qmin = cur_Q;
                        result.Qmax = cur_Q;
                        first_step_flag = false;
                    }
                    else
                    {
                        Support.MinAndMaxFinder(cur_P, ref result.Pmin, ref result.Pmax);
                        Support.MinAndMaxFinder(cur_Q, ref result.Qmin, ref result.Qmax);
                    }
                    result.P += cur_P;
                    result.Q += cur_Q;
                    result.Psq += Math.Pow(cur_P * 12000, 2) * time_interval; //далее понадобится для расчета среднеквадратичных значений
                    result.Qsq += Math.Pow(cur_Q * 12000, 2) * time_interval; //далее понадобится для расчета среднеквадратичных значений
                    result.periods++;
                }
            }
            if (result.periods == 0) Console.WriteLine("Неверно указан период (месяц или год) или данный период отсутствует в документе");
            return result;
        }*/
    }
}
