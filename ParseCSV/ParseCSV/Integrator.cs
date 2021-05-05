using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    struct Meter
    {
        public double SumRowsP;  //сумма всех ячеек активной мощности за выбранный период (для проверки)
        public double SumRowsQ;  //сумма всех ячеек реактивной мощности за выбранный период (для проверки)
        public double Psq;       //среднеквадратичное значение активной мощности за выбранный период (месяц)
        public double Qsq;       //среднеквадратичное значение реактивной мощности за выбранный период (месяц)
        public double MaxP;      //максимальное среднее значение потребления активной мощности за выбранный период (месяц), используется не приведенное значения для простоты проверки
        public double MinP;      //минимальное ------//------
        public double MaxQ;      //максимальное среднее значение потребления реактивной мощности за выбранный период (месяц), используется не приведенное значения для простоты проверки
        public double MinQ;      //минимальное ------//------
        public double TotalMin;  //время (в минутах) общей наработки счетчика за выбранный период (месяц)
    }
    
    class Integrator
    {
        public Integrator(string path, string month, int year) { p = path; mn = month; y = year; } // конструктор
        string p;
        string mn;
        int m;
        int y;
        string[,] array;
        (int, int) range;
        public Meter data;
        public int ratio { get; set; } = 12000;
        public string path_out_f = Directory.GetCurrentDirectory() + "\\output.CSV";
        public void CookFile()
        {
            bool first_iteration_flag = true;
            double cur_p = 0;
            double cur_q = 0;
            double temp_Psq = 0;
            double temp_Qsq = 0;
            m = Helper.GetMonthNumber(mn);
            array = Helper.GetArrayFromCsvFile(p);
            range = Helper.GetRowsRangeByMonth(array, m, y);
            try
            {
                for (int i = range.Item1; i < range.Item2 + 1; i++)
                {
                    cur_p = double.Parse(array[3, i]);
                    cur_q = double.Parse(array[17, i]);
                    data.SumRowsP += cur_p;
                    data.SumRowsQ += cur_q;
                    double min_interval = Helper.GetMinuteInterval(array[1, i], array[2, i]);
                    data.TotalMin += min_interval;
                    temp_Psq += Math.Pow(cur_p * ratio, 2) * min_interval;
                    temp_Qsq += Math.Pow(cur_q * ratio, 2) * min_interval;
                    if (first_iteration_flag == true)
                    {
                        data.MaxP = cur_p;
                        data.MinP = cur_p;
                        data.MaxQ = cur_q;
                        data.MinQ = cur_q;
                        first_iteration_flag = false;
                    }
                    else
                    {
                        Helper.GetMinAndMax(cur_p, ref data.MinP, ref data.MaxP);
                        Helper.GetMinAndMax(cur_q, ref data.MinQ, ref data.MaxQ);
                    }                    
                }
                data.Psq = Math.Sqrt(temp_Psq / data.TotalMin);
                data.Qsq = Math.Sqrt(temp_Qsq / data.TotalMin);
                string[] collunms_name = { "SumRowsP", "SumRowsQ", "Psq", "Qsq", "MaxP", "MinP", "MaxQ", "MinQ", "TotalMin" };
                double[] val_array = { data.SumRowsP, data.SumRowsQ, data.Psq, data.Qsq, data.MaxP, data.MinP, data.MaxQ, data.MinQ, data.TotalMin };
                Helper.MakeCsvFile(path_out_f, collunms_name, val_array);
            }
            catch (Exception)
            {

                Console.WriteLine("Нет релевантных данных");
            }
        }
        public void GetInfo()
        {
            Console.WriteLine(path_out_f);
            Console.WriteLine(m);
            Console.WriteLine(y);
        }
    }
}
