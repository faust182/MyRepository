using System;
using System.IO;
using System.Text;

namespace ParseCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV";
            string[,] array = Support.GetArrayFromCsvFile(path);
            var tuple = Support.GetRowsRangeByMonth(array, 3, 2017);
            Console.WriteLine(tuple.start);
            Console.WriteLine(tuple.end);
            Integrator a = new Integrator(path, 3, 2017);
            a.
            /* Console.WriteLine(@"Ведите полный путь до файла с показаниями счетчика
 (пример: d:\Work\...\Example.CSV):");

             string path = Console.ReadLine();
             //string cur_directory = Directory.GetCurrentDirectory();
             string[] cur_path = path.Split('\\');
             int ratio = 12000; // будет перемножаться с double, можно использовать int? у меня такое дальше будет, значения не искажаются
             var P = new double();// var
             var Q = new double();
             var Pmax = new double();
             var Pmin = new double();
             var Qmax = new double();
             var Qmin = new double();
             var Psq = new double();
             var Qsq = new double();
             int year = new int();

             StringBuilder path_out_f = new StringBuilder();
             for (int i = 0; i < cur_path.Length - 1; i++)
             {
                 path_out_f.Append(cur_path[i] + '\\');
             }
             path_out_f.Append("output.CSV");//можно было разместить файл в директории запуска приложения, но я решил так.
             Console.WriteLine(path_out_f + "- здесь будет находиться файл");
             //d:\Anton\Work\C#\ParseCSV\ParseCSV\Example meters.CSV
             Console.WriteLine(@"Укажите год (пример: 2001):");
             try
             {
                 year = int.Parse(Console.ReadLine());
             }
             catch (Exception)
             {
                 Console.WriteLine("Неверно введен год");
             }
             Console.WriteLine(@"Укажите название месяца или его номер(указывается порядковый номер месяца. Январь - 1, декабрь - 12):");
             string str_month = Console.ReadLine().ToLower();
             int month = Support.GetMonthNumber(str_month);
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

             //а какие еще есть способы кроме ref и out для работы с "внешними переменными"??? 
             var out_tuple = Support.Integrator(month, year, number_rows, data_array);
             P = out_tuple.sum_P * ratio; //бессмысленное значение, просто для проверки с исходным файлом
             Q = out_tuple.sum_Q * ratio; //бессмысленное значение, просто для проверки с исходным файлом
             Pmax = out_tuple.max_P * ratio; //максимальное значение потребления активной мощности за период
             Pmin = out_tuple.min_P * ratio; //минимальное значение потребления реактивной мощности за период
             Qmax = out_tuple.max_Q * ratio; //максимальное значение потребления реактивной мощности за период
             Qmin = out_tuple.min_Q * ratio; //минимальное значение потребления реактивной мощности за период
             Psq = Math.Sqrt(out_tuple.sq_P / out_tuple.sum_minutes); //среднеквадратичное значение aктивной мощности
             Qsq = Math.Sqrt(out_tuple.sq_Q / out_tuple.sum_minutes); //среднеквадратичное значение реактивной мощности

             try
             {
                 using (StreamWriter sw = new StreamWriter(path_out_f.ToString(), false, System.Text.Encoding.Default))
                 {
                     sw.WriteLine("P;Q;Pmax;Pmin;Qmax;Qmin;Psq;Qsq");
                     sw.Write(P.ToString() + ';');
                     sw.Write(Q.ToString() + ';');
                     sw.Write(Pmax.ToString() + ';');
                     sw.Write(Pmin.ToString() + ';');
                     sw.Write(Qmax.ToString() + ';');
                     sw.Write(Qmin.ToString() + ';');
                     sw.Write(Psq.ToString() + ';');
                     sw.Write(Qsq.ToString());
                 }
                 Console.WriteLine("Запись выполнена. Смотрите результат в файле output.CSV");
             }
             catch (Exception e)
             {
                 Console.WriteLine(e.Message);
             }*/
        }
    }
}
