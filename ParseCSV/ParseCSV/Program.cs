using System;
using System.IO;
using System.Text;

namespace ParseCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Ведите полный путь до файла с показаниями счетчика
(пример: d:\Work\...\Example.CSV):");
            string path = Console.ReadLine();
            string[] current_path = path.Split('\\');
            double ratio = 12000; // будет перемножаться с double, можно использовать int? у меня такое дальше будет, значения не искажаются
            double reduced_value_P = new double();
            double reduced_value_Q = new double();
            double reduced_Pmax = new double();
            double reduced_Pmin = new double();
            double reduced_Qmax = new double();
            double reduced_Qmin = new double();
            double Psq = new double();
            double Qsq = new double();

            StringBuilder path_for_output_file = new StringBuilder();
            for (int i = 0; i < current_path.Length - 1; i++)
            {
                path_for_output_file.Append(current_path[i] + '\\');
            }
            path_for_output_file.Append("output.CSV");//можно было разместить файл в директории запуска приложения, но я решил так.
            Console.WriteLine(path_for_output_file + "- здесь будет находиться файл");
            //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
            Console.WriteLine(@"Укажите год (пример: 2001):");
            int year = int.Parse(Console.ReadLine());
            Console.WriteLine(@"Укажите название месяца или его номер(указывается порядковый номер месяца. Январь - 1, декабрь - 12):");
            string str_month = Console.ReadLine().ToLower();
            int month = WhatIsMonth(str_month);
            StringBuilder input = new StringBuilder();
            int number_rows = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                string line;//наверно, нехорошо использовать тут string, но в примере на metanit так...
                while ((line = sr.ReadLine()) != null)
                {
                    input.Append(line + '\n');
                    number_rows++;
                }
            }

            string[] input_string_array = input.ToString().Split('\n');
            int number_columns = input_string_array[0].Split(';').Length;
            string[,] data_array = new string[number_columns, number_rows];

            for (int i = 0; i < number_rows; i++)
            {
                string[] temp_array = input_string_array[i].Split(';');
                for (int j = 0; j < number_columns; j++)
                {
                    data_array[j, i] = temp_array[j];
                }
            }

            static void MinAndMaxFinder(double val, ref double min, ref double max)
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

            static int WhatIsMonth(string month)
            {
                if (month == "январь" || month == "1")
                {
                    return 1;
                }
                else if (month == "февраль" || month == "2")
                {
                    return 2;
                }
                else if (month == "март" || month == "3")
                {
                    return 3;
                }
                else if (month == "апрель" || month == "4")
                {
                    return 4;
                }
                else if (month == "май" || month == "5")
                {
                    return 5;
                }
                else if (month == "июнь" || month == "6")
                {
                    return 6;
                }
                else if (month == "июль" || month == "7")
                {
                    return 7;
                }
                else if (month == "август" || month == "8")
                {
                    return 8;
                }
                else if (month == "сентябрь" || month == "9")
                {
                    return 9;
                }
                else if (month == "октябрь" || month == "10")
                {
                    return 10;
                }
                else if (month == "ноябрь" || month == "11")
                {
                    return 11;
                }
                else if (month == "декабрь" || month == "12")
                {
                    return 12;
                }
                else
                {
                    Console.WriteLine("Неверно указан месяц");
                    return 0;
                }
            }

            static (int hour, int minute, int second) WhatIsTimeInterval(string StartTime, string EndTime) //это не универсальный метод (при переходе интервала в полночь он выдаст неверные данные),
                                                                                                           //он для данной задачи но походит
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

            static (int day, int month, int year) DateParser(string date)//просто преобразует формат даты в значения Int
            {
                var result = (day: 0, month: 0, year: 0);
                string[] input_date = date.Split('.');
                if (input_date.Length == 3) //небольшая защита от неподходящего формата данных
                {
                    result.day = int.Parse(input_date[0]);
                    result.month = int.Parse(input_date[1]);
                    result.year = int.Parse(input_date[2]);
                }
                return result;
            }

            static (double sum_P, double sum_Q, double max_P, double min_P, double max_Q, double min_Q, double sq_P, double sq_Q, int sum_minutes, int sum_periods) Integrator
            (int month, int year, int number_rows, string[,] data_array)
            {
                bool first_step_flag = true; //костыль, чтобы при первом проходе присвоить какое-то значение result.Pmin и result.Qmin,
                                             //иначе они остануться равны проинициализированным "0", т.к. в источнике все значения больше нуля
                var result = (P: 0.0, Q: 0.0, Pmax: 0.0, Pmin: 0.0, Qmax: 0.0, Qmin: 0.0, Psq: 0.0, Qsq: 0.0, minutes: 0, periods: 0);
                for (int i = 0; i < number_rows; i++)
                {
                    var current_date = DateParser(data_array[0, i]);
                    if (current_date.month == month && current_date.year == year)
                    {
                        var time_interval = WhatIsTimeInterval(data_array[1, i], data_array[2, i]);
                        result.minutes += time_interval.minute;
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
                            MinAndMaxFinder(cur_P, ref result.Pmin, ref result.Pmax);
                            MinAndMaxFinder(cur_Q, ref result.Qmin, ref result.Qmax);
                        }
                        result.P += cur_P;
                        result.Q += cur_Q;
                        result.Psq += Math.Pow(cur_P * 12000, 2) * time_interval.minute; //далее понадобится для расчета среднеквадратичных значений
                        result.Qsq += Math.Pow(cur_Q * 12000, 2) * time_interval.minute; //далее понадобится для расчета среднеквадратичных значений
                        result.periods++;
                    }
                }
                if (result.periods == 0) Console.WriteLine("Неверно указан период (месяц или год) или данный период отсутствует в документе");
                return result;
            }

            //а какие еще есть способы кроме ref и out для работы с "внешними переменными"??? 
            var out_tuple = Integrator(month, year, number_rows, data_array);
            reduced_value_P = out_tuple.sum_P * ratio; //бессмысленное значение, просто для проверки с исходным файлом
            reduced_value_Q = out_tuple.sum_Q * ratio; //бессмысленное значение, просто для проверки с исходным файлом
            reduced_Pmax = out_tuple.max_P * ratio; //максимальное значение потребления активной мощности за период 
            reduced_Pmin = out_tuple.min_P * ratio; //минимальное значение потребления реактивной мощности за период
            reduced_Qmax = out_tuple.max_Q * ratio; //максимальное значение потребления реактивной мощности за период
            reduced_Qmin = out_tuple.min_Q * ratio; //минимальное значение потребления реактивной мощности за период
            Psq = Math.Sqrt(out_tuple.sq_P / out_tuple.sum_minutes); //среднеквадратичное значение aктивной мощности
            Qsq = Math.Sqrt(out_tuple.sq_Q / out_tuple.sum_minutes); //среднеквадратичное значение реактивной мощности

            try
            {
                using (StreamWriter sw = new StreamWriter(path_for_output_file.ToString(), false, System.Text.Encoding.Default))
                {
                    sw.WriteLine("P;Q;Pmax;Pmin;Qmax;Qmin;Psq;Qsq");
                    sw.Write(reduced_value_P.ToString() + ';');
                    sw.Write(reduced_value_Q.ToString() + ';');
                    sw.Write(reduced_Pmax.ToString() + ';');
                    sw.Write(reduced_Pmin.ToString() + ';');
                    sw.Write(reduced_Qmax.ToString() + ';');
                    sw.Write(reduced_Qmin.ToString() + ';');
                    sw.Write(Psq.ToString() + ';');
                    sw.Write(Qsq.ToString());
                }
                Console.WriteLine("Запись выполнена. Смотрите результат в файле output.CSV");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
