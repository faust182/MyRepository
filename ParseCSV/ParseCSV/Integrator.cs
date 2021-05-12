using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    struct InputData
    {
        public string PathInputFile;
        public string PathOutputFile;
        public string Month;
        public int Year;
    }
    struct OutputData
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
        public List<DateTime> ColumnDate;
        public List<DateTime> ColumnStartTime;
        public List<DateTime> ColumnEndTime;
        public List<double> ColumnActivePower;
        public List<double> ColumnReactivePower;
        public Table()
        {
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
    class Integrator
    {
        public Integrator(string path, string inputMonth, int inputYear) { path = pathToInputFile; inputMonth = month; inputYear = year; } // конструктор
        const string defaultNameOutputFile = "\\output.CSV";
        string pathToInputFile;
        string month;
        int year;
        public int ratio { get; set; } = 12000;
        public string pathForOutFile = Directory.GetCurrentDirectory() + defaultNameOutputFile;

        public void CreateOutputFile()
        {
            var outputRow = new OutputData();
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
