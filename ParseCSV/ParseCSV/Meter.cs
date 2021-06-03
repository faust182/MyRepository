using System;
using System.Collections.Generic;
using System.IO;
using static ParseCSV.Constants;

namespace ParseCSV
{
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

    class Meter
    {
        public string PathForOutFile = Directory.GetCurrentDirectory() + DefaultNameOutputFile;

        public OutputData OutputRow = new OutputData();

        private bool flagSuccessReadFile = true;

        public TableConstructor.Table Table2 { get; set; }

        public int Ratio { get; set; } = DefaultRatio;

        InputData Input { get; set; }

        public void GetData()
        {
            Input = Helper.GetInputData();
        }

        public void CalculateValues()
        {
            double currenValueOfActivePower = 0;
            double currenValueOfReactivePower = 0;
            double currentTimeInterval = 0;
            double tempRmsActivePower = 0;
            double tempRmsReactivePower = 0;
            Range relevantRageOfRows;
            Table tableFromInputFile;
            var inputList = Helper.ReadCsv(Input.PathInputFile);
            if (inputList.Count == 0) 
            {
                flagSuccessReadFile = false;
                return;
            }

            do
            {
                tableFromInputFile = Helper.ParseCsv(inputList);
                Table2 = Helper.ParseCsv2(inputList);
                int numberOfMomth = Helper.GetMonthNumber(Input.Month);
                relevantRageOfRows = Helper.GetRowsRangeByMonthOfYear(tableFromInputFile, numberOfMomth, Input.Year);
                if (relevantRageOfRows.Start == int.MinValue)
                {
                    Console.WriteLine("В документе нет запрашиваемого диапазона по дате");
                    Console.WriteLine("Повторите ввод данных");
                    Console.WriteLine();
                    Input = Helper.GetInputData();
                }
            }
            while (relevantRageOfRows.Start == int.MinValue);
            
            OutputRow.MaxP = Helper.GetMaxInRange(tableFromInputFile.ColumnActivePower, relevantRageOfRows);
            OutputRow.MaxQ = Helper.GetMaxInRange(tableFromInputFile.ColumnReactivePower, relevantRageOfRows);
            OutputRow.MinP = Helper.GetMinInRange(tableFromInputFile.ColumnActivePower, relevantRageOfRows);
            OutputRow.MinQ = Helper.GetMinInRange(tableFromInputFile.ColumnReactivePower, relevantRageOfRows);
            for (int i = relevantRageOfRows.Start; i <= relevantRageOfRows.End; i++)
            {
                currenValueOfActivePower = tableFromInputFile.ColumnActivePower[i];
                currenValueOfReactivePower = tableFromInputFile.ColumnReactivePower[i];
                OutputRow.SumRowsP += currenValueOfActivePower;
                OutputRow.SumRowsQ += currenValueOfReactivePower;
                currentTimeInterval = Helper.GetTimeMinuteInterval(tableFromInputFile.ColumnStartTime[i], tableFromInputFile.ColumnEndTime[i]);
                OutputRow.TotalMin += currentTimeInterval;
                tempRmsActivePower += Math.Pow(currenValueOfActivePower * Ratio, 2) * currentTimeInterval;
                tempRmsReactivePower += Math.Pow(currenValueOfReactivePower * Ratio, 2) * currentTimeInterval;
            }

            OutputRow.Prms = Math.Sqrt(tempRmsActivePower / OutputRow.TotalMin);
            OutputRow.Qrms = Math.Sqrt(tempRmsReactivePower / OutputRow.TotalMin);
        }

        public void CreateOutputFile()
        {
            if (flagSuccessReadFile)
            {
                string[] collumnsName = { "SumRowsP", "SumRowsQ", "Prms", "Qrms", "MaxP", "MinP", "MaxQ", "MinQ", "TotalMin" };
                double[] valArray = { OutputRow.SumRowsP, OutputRow.SumRowsQ, OutputRow.Prms, OutputRow.Qrms, OutputRow.MaxP, OutputRow.MinP, OutputRow.MaxQ, OutputRow.MinQ, OutputRow.TotalMin };
                if (string.IsNullOrEmpty(Input.PathOutputFile)) Helper.CreateCsvFile(PathForOutFile, collumnsName, valArray);
                else Helper.CreateCsvFile(Input.PathOutputFile, collumnsName, valArray);
            }
            else
            {
                Console.WriteLine("Создание файла невозможно");
            }
        }
    }
}
