using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    struct Meter
    {
        /// <summary>
        /// сумма всех ячеек активной мощности за выбранный период (для проверки)
        /// </summary>
        public double SumRowsP;
        /// <summary>
        /// сумма всех ячеек реактивной мощности за выбранный период (для проверки)
        /// </summary>
        public double SumRowsQ;
        /// <summary>
        /// среднеквадратичное значение активной мощности за выбранный период (месяц)
        /// </summary>
        public double Prms;
        /// <summary>
        /// среднеквадратичное значение реактивной мощности за выбранный период (месяц)
        /// </summary>
        public double Qrms;
        /// <summary>
        /// максимальное среднее значение потребления активной мощности за выбранный период (месяц), используется не приведенное значения для простоты проверки
        /// </summary>
        public double MaxP;
        /// <summary>
        /// минимальное среднее значение потребления активной мощности за выбранный период (месяц), используется не приведенное значения для простоты проверки
        /// </summary>
        public double MinP;
        /// <summary>
        /// максимальное среднее значение потребления реактивной мощности за выбранный период (месяц), используется не приведенное значения для простоты проверки
        /// </summary>
        public double MaxQ;
        /// <summary>
        /// минимальное среднее значение потребления реактивной мощности за выбранный период (месяц), используется не приведенное значения для простоты проверки
        /// </summary>
        public double MinQ;
        /// <summary>
        /// время (в минутах) общей наработки счетчика за выбранный период (месяц)
        /// </summary>
        public double TotalMin;
    }
    struct Range
    {
        public int Start;
        public int End;
    }
    class Row
    {
        public DateTime Date;
        public DateTime StartTime;
        public DateTime EndTime;
        public double ActivePower;
        public double ReactivePower;
    }
    class Table
    {
        public bool initialize;
        public List<DateTime> ColumnDate;
        public List<DateTime> ColumnStartTime;
        public List<DateTime> ColumnEndTime;
        public List<double> ColumnActivePower;
        public List<double> ColumnReactivePower;
        public Table(bool init)
        {
            initialize = init;
            ColumnDate = new List<DateTime>();
            ColumnStartTime = new List<DateTime>();
            ColumnEndTime = new List<DateTime>();
            ColumnActivePower = new List<double>();
            ColumnReactivePower = new List<double>();
        }
        public Row GetRow(int numberRow)
        {
            var row = new Row();
            row.Date = ColumnDate[numberRow];
            row.StartTime = ColumnStartTime[numberRow];
            row.EndTime = ColumnEndTime[numberRow];
            row.ActivePower = ColumnActivePower[numberRow];
            row.ReactivePower = ColumnReactivePower[numberRow];
            return row;
        }
    }

    class FirstIntegrator
    {
        public FirstIntegrator(string path, string month, int year) { p = path; mn = month; y = year; } // конструктор
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
                data.Prms = Math.Sqrt(temp_Psq / data.TotalMin);
                data.Qrms = Math.Sqrt(temp_Qsq / data.TotalMin);
                string[] collunms_name = { "SumRowsP", "SumRowsQ", "Psq", "Qsq", "MaxP", "MinP", "MaxQ", "MinQ", "TotalMin" };
                double[] val_array = { data.SumRowsP, data.SumRowsQ, data.Prms, data.Qrms, data.MaxP, data.MinP, data.MaxQ, data.MinQ, data.TotalMin };
                Helper.CriateCsvFile(path_out_f, collunms_name, val_array);
            }
            catch (Exception)
            {
                Console.WriteLine("Нет релевантных данных");
            }
        }
    }
    class SecondIntegrator
    {
        public SecondIntegrator(string _pathToInputFile, string _month, int _year) { pathToInputFile = _pathToInputFile; month = _month; year = _year; } // конструктор
        const string defaultNameOutputFile = "\\output.CSV";
        string pathToInputFile;
        string month;
        int year;
        public int ratio { get; set; } = 12000;
        public string pathForOutFile = Directory.GetCurrentDirectory() + defaultNameOutputFile;
        //var tableFromInputFile = new Table(true);

        public void CookOutputFile()
        {
            var outputRow = new Meter();
            double currenValueOfActivePower = 0;
            double currenValueOfReactivePower = 0;
            double currentTimeInterval = 0;
            double tempRmsActivePower = 0;
            double tempRmsReactivePower = 0;
            Table tableFromInputFile = Helper.GetStructFromInputCsvFile(pathToInputFile);
            int numberOfMomth = Helper.GetMonthNumber(month);
            var range = Helper.GetRowsRangeByMonthOfYear(tableFromInputFile, numberOfMomth, year);
            outputRow.MaxP = Helper.GetMaxInRange(tableFromInputFile.ColumnActivePower, range);
            outputRow.MaxQ = Helper.GetMaxInRange(tableFromInputFile.ColumnReactivePower, range);
            outputRow.MinP = Helper.GetMinInRange(tableFromInputFile.ColumnActivePower, range);
            outputRow.MinQ = Helper.GetMinInRange(tableFromInputFile.ColumnReactivePower, range);
            for (int i = range.Start; i <= range.End; i++)
            {
                currenValueOfActivePower = tableFromInputFile.ColumnActivePower[i];
                currenValueOfReactivePower = tableFromInputFile.ColumnReactivePower[i];
                outputRow.SumRowsP += currenValueOfActivePower;
                outputRow.SumRowsQ += currenValueOfReactivePower;
                currentTimeInterval = Helper.GetTimeMinuteInterval(tableFromInputFile.ColumnStartTime[i], tableFromInputFile.ColumnEndTime[i]);
                outputRow.TotalMin += currentTimeInterval;
                tempRmsActivePower += Math.Pow(currenValueOfActivePower * ratio, 2) * currentTimeInterval;
                tempRmsReactivePower += Math.Pow(currenValueOfReactivePower * ratio, 2) * currentTimeInterval;
            }
            outputRow.Prms = Math.Sqrt(tempRmsActivePower / outputRow.TotalMin);
            outputRow.Qrms = Math.Sqrt(tempRmsReactivePower / outputRow.TotalMin);
            string[] collumnsName = { "SumRowsP", "SumRowsQ", "Prms", "Qrms", "MaxP", "MinP", "MaxQ", "MinQ", "TotalMin" };
            double[] valArray = { outputRow.SumRowsP, outputRow.SumRowsQ, outputRow.Prms, outputRow.Qrms, outputRow.MaxP, outputRow.MinP, outputRow.MaxQ, outputRow.MinQ, outputRow.TotalMin };
            Helper.CriateCsvFile(pathForOutFile, collumnsName, valArray);
        }
    }
}
