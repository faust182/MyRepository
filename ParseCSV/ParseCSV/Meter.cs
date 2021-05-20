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
    class Table
    {
        public List<DateTime> columnDate { get; set; }
        public List<DateTime> columnStartTime { get; set; }
        public List<DateTime> columnEndTime { get; set; }
        public List<double> columnActivePower { get; set; }
        public List<double> columnReactivePower { get; set; }
        public Table()
        {
            this.columnDate = new List<DateTime>();
            this.columnStartTime = new List<DateTime>();
            this.columnEndTime = new List<DateTime>();
            this.columnActivePower = new List<double>();
            this.columnReactivePower = new List<double>();
        }
    }
    class Meter
    {

        /*public Meter(string path, string inputMonth, int inputYear)
        { 
            pathToInputFile = path; 
            month = inputMonth; 
            year = inputYear; 
        }*/
        /*string pathToInputFile { get; set; }
        string month { get; set; }
        int year { get; set; }*/
        InputData input { get; set; }
        public int ratio { get; set; } = DefaultRatio;
        private bool flagSuccessReadFile = true;
        public string pathForOutFile = Directory.GetCurrentDirectory() + DefaultNameOutputFile;
        public OutputData outputRow = new OutputData();

        public void GetData ()
        {
            input = Helper.GetInputData();
        }

        public void CalculateValues()
        {
            double currenValueOfActivePower = 0;
            double currenValueOfReactivePower = 0;
            double currentTimeInterval = 0;
            double tempRmsActivePower = 0;
            double tempRmsReactivePower = 0;
            var inputList = Helper.ReadeCsv(input.pathInputFile);
            if (inputList.Count == 0) 
            {
                flagSuccessReadFile = false;
                return;
            }
            Table tableFromInputFile = Helper.ParseCsv(inputList);
            int numberOfMomth = Helper.GetMonthNumber(input.month);
            var range = Helper.GetRowsRangeByMonthOfYear(tableFromInputFile, numberOfMomth, input.year);
            outputRow.MaxP = Helper.GetMaxInRange(tableFromInputFile.columnActivePower, range);
            outputRow.MaxQ = Helper.GetMaxInRange(tableFromInputFile.columnReactivePower, range);
            outputRow.MinP = Helper.GetMinInRange(tableFromInputFile.columnActivePower, range);
            outputRow.MinQ = Helper.GetMinInRange(tableFromInputFile.columnReactivePower, range);
            for (int i = range.Start; i <= range.End; i++)
            {
                currenValueOfActivePower = tableFromInputFile.columnActivePower[i];
                currenValueOfReactivePower = tableFromInputFile.columnReactivePower[i];
                outputRow.SumRowsP += currenValueOfActivePower;
                outputRow.SumRowsQ += currenValueOfReactivePower;
                currentTimeInterval = Helper.GetTimeMinuteInterval(tableFromInputFile.columnStartTime[i], tableFromInputFile.columnEndTime[i]);
                outputRow.TotalMin += currentTimeInterval;
                tempRmsActivePower += Math.Pow(currenValueOfActivePower * ratio, 2) * currentTimeInterval;
                tempRmsReactivePower += Math.Pow(currenValueOfReactivePower * ratio, 2) * currentTimeInterval;
            }
            outputRow.Prms = Math.Sqrt(tempRmsActivePower / outputRow.TotalMin);
            outputRow.Qrms = Math.Sqrt(tempRmsReactivePower / outputRow.TotalMin);
        }


        public void CreateOutputFile()
        {
            if (flagSuccessReadFile)
            {
                string[] collumnsName = { "SumRowsP", "SumRowsQ", "Prms", "Qrms", "MaxP", "MinP", "MaxQ", "MinQ", "TotalMin" };
                double[] valArray = { outputRow.SumRowsP, outputRow.SumRowsQ, outputRow.Prms, outputRow.Qrms, outputRow.MaxP, outputRow.MinP, outputRow.MaxQ, outputRow.MinQ, outputRow.TotalMin };
                Helper.CreateCsvFile(pathForOutFile, collumnsName, valArray);
            }
            else
            {
                Console.WriteLine("Создание файла невозможно");
            }
        }
    }
}
