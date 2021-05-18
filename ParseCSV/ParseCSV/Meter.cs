using System;
using System.IO;
using System.Collections.Generic;
using static ParseCSV.Constants;

namespace ParseCSV
{
    class InputData
    {
        public string pathInputFile;
        public string pathOutputFile;
        public string month;
        public int year;
    }
    struct OutputData
    {
        /// <summary>
        /// сумма всех ячеек активной мощности за выбранный период (для проверки)
        /// </summary>
        public double sumRowsP;
        /// <summary>
        /// сумма всех ячеек реактивной мощности за выбранный период (для проверки)
        /// </summary>
        public double sumRowsQ;
        /// <summary>
        /// среднеквадратичное значение активной мощности за выбранный период (месяц)
        /// </summary>
        public double pRms;
        /// <summary>
        /// среднеквадратичное значение реактивной мощности за выбранный период (месяц)
        /// </summary>
        public double qRms;
        /// <summary>
        /// максимальное среднее значение потребления активной мощности за выбранный период (месяц), используется не приведенное значения для простоты проверки
        /// </summary>
        public double maxP;
        /// <summary>
        /// минимальное среднее значение потребления активной мощности за выбранный период (месяц), используется не приведенное значения для простоты проверки
        /// </summary>
        public double minP;
        /// <summary>
        /// максимальное среднее значение потребления реактивной мощности за выбранный период (месяц), используется не приведенное значения для простоты проверки
        /// </summary>
        public double maxQ;
        /// <summary>
        /// минимальное среднее значение потребления реактивной мощности за выбранный период (месяц), используется не приведенное значения для простоты проверки
        /// </summary>
        public double minQ;
        /// <summary>
        /// время (в минутах) общей наработки счетчика за выбранный период (месяц)
        /// </summary>
        public double totalMin;
    }
    struct Range
    {
        public int start;
        public int end;
    }
    class Table
    {
        public List<DateTime> columnDate { get; set; }
        public List<DateTime> columnStartTime { get; set; }
        public List<DateTime> columnEndTime { get; set; }
        public List<double> columnActivePower { get; set; }
        public List<double> columnReactivePower { get; set; }
        public Table()
        {
            columnDate = new List<DateTime>();
            columnStartTime = new List<DateTime>();
            columnEndTime = new List<DateTime>();
            columnActivePower = new List<double>();
            columnReactivePower = new List<double>();
        }
    }
    class Meter
    {
        public Meter(string path, string inputMonth, int inputYear)
        { 
            pathToInputFile = path; 
            month = inputMonth; 
            year = inputYear; 
        }
        string pathToInputFile;
        string month;
        int year;
        public int ratio { get; set; } = 12000;
        public string pathForOutFile = Directory.GetCurrentDirectory() + DefaultNameOutputFile;


        public void CreateOutputFile()
        {
            var outputRow = new OutputData();
            double currenValueOfActivePower = 0;
            double currenValueOfReactivePower = 0;
            double currentTimeInterval = 0;
            double tempRmsActivePower = 0;
            double tempRmsReactivePower = 0;
            var inputList = Helper.Reader(pathToInputFile);
            Table tableFromInputFile = Helper.Parser(inputList);
            int numberOfMomth = Helper.GetMonthNumber(month);
            var range = Helper.GetRowsRangeByMonthOfYear(tableFromInputFile, numberOfMomth, year);
            outputRow.maxP = Helper.GetMaxInRange(tableFromInputFile.columnActivePower, range);
            outputRow.maxQ = Helper.GetMaxInRange(tableFromInputFile.columnReactivePower, range);
            outputRow.minP = Helper.GetMinInRange(tableFromInputFile.columnActivePower, range);
            outputRow.minQ = Helper.GetMinInRange(tableFromInputFile.columnReactivePower, range);
            for (int i = range.start; i <= range.end; i++)
            {
                currenValueOfActivePower = tableFromInputFile.columnActivePower[i];
                currenValueOfReactivePower = tableFromInputFile.columnReactivePower[i];
                outputRow.sumRowsP += currenValueOfActivePower;
                outputRow.sumRowsQ += currenValueOfReactivePower;
                currentTimeInterval = Helper.GetTimeMinuteInterval(tableFromInputFile.columnStartTime[i], tableFromInputFile.columnEndTime[i]);
                outputRow.totalMin += currentTimeInterval;
                tempRmsActivePower += Math.Pow(currenValueOfActivePower * ratio, 2) * currentTimeInterval;
                tempRmsReactivePower += Math.Pow(currenValueOfReactivePower * ratio, 2) * currentTimeInterval;
            }
            outputRow.pRms = Math.Sqrt(tempRmsActivePower / outputRow.totalMin);
            outputRow.qRms = Math.Sqrt(tempRmsReactivePower / outputRow.totalMin);
            string[] collumnsName = { "SumRowsP", "SumRowsQ", "Prms", "Qrms", "MaxP", "MinP", "MaxQ", "MinQ", "TotalMin" };
            double[] valArray = { outputRow.sumRowsP, outputRow.sumRowsQ, outputRow.pRms, outputRow.qRms, outputRow.maxP, outputRow.minP, outputRow.maxQ, outputRow.minQ, outputRow.totalMin };
            Helper.CreateCsvFile(pathForOutFile, collumnsName, valArray);
        }
    }
}
