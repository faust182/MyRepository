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
        OutputData outputRow = new OutputData();

        WorkConfig userInput = new WorkConfig();

        bool isReadingOfFileSuccessful = true;

        public string DefaultPathForOutputFile { get; set; } = Directory.GetCurrentDirectory() + DefaultNameOutputFile;

        public MyTable TableFromInputFile { get; set; }

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
            Range rangeOfRows;
            var inputList = Helper.ReadCsv(Input.PathInputFile);
            if (inputList.Count == 0) 
            {
                isReadingOfFileSuccessful = false;
                return;
            }

            do
            {
                TableFromInputFile = Helper.ParseCsv(inputList);
                int numberOfMomth = Helper.GetMonthNumber(Input.Month);
                rangeOfRows = Helper.GetRowsRangeByMonthOfYear(TableFromInputFile, numberOfMomth, Input.Year);
                if (rangeOfRows.Start == int.MinValue)
                {
                    Console.WriteLine("В документе нет запрашиваемого диапазона по дате");
                    Console.WriteLine(
                        "В документе содержатся данные в диапазоне с {0} по {1}", 
                        TableFromInputFile[0].Date.ToShortDateString(),
                        TableFromInputFile[TableFromInputFile.Table.Count - 1].Date.ToShortDateString());

                    Console.WriteLine("Повторите ввод данных");
                    Console.WriteLine();
                    Input.GetYear(userInput);
                    Input.GetMonth(userInput);
                    Console.WriteLine();
                }
            }
            while (rangeOfRows.Start == int.MinValue);
            
            outputRow.MaxP = Helper.GetMaxInRange(TableFromInputFile, rangeOfRows, TypeOfPower.ActivePower);
            outputRow.MaxQ = Helper.GetMaxInRange(TableFromInputFile, rangeOfRows, TypeOfPower.ReactivePower);
            outputRow.MinP = Helper.GetMinInRange(TableFromInputFile, rangeOfRows, TypeOfPower.ActivePower);
            outputRow.MinQ = Helper.GetMinInRange(TableFromInputFile, rangeOfRows, TypeOfPower.ReactivePower);
            for (int i = rangeOfRows.Start; i <= rangeOfRows.End; i++)
            {
                currenValueOfActivePower = TableFromInputFile[i].ActivePower;
                currenValueOfReactivePower = TableFromInputFile[i].ReactivePower;
                outputRow.SumRowsP += currenValueOfActivePower;
                outputRow.SumRowsQ += currenValueOfReactivePower;
                currentTimeInterval = Helper.GetTimeMinuteInterval(TableFromInputFile[i].StartTime, TableFromInputFile[i].EndTime);
                outputRow.TotalMin += currentTimeInterval;
                tempRmsActivePower += Math.Pow(currenValueOfActivePower * Ratio, 2) * currentTimeInterval;
                tempRmsReactivePower += Math.Pow(currenValueOfReactivePower * Ratio, 2) * currentTimeInterval;
            }

            outputRow.Prms = Math.Sqrt(tempRmsActivePower / outputRow.TotalMin);
            outputRow.Qrms = Math.Sqrt(tempRmsReactivePower / outputRow.TotalMin);
        }

        public void CreateOutputFile()
        {
            if (isReadingOfFileSuccessful)
            {
                string[] collumnsName = { "SumRowsP", "SumRowsQ", "Prms", "Qrms", "MaxP", "MinP", "MaxQ", "MinQ", "TotalMin" };
                double[] valArray = { outputRow.SumRowsP, outputRow.SumRowsQ, outputRow.Prms, outputRow.Qrms, outputRow.MaxP, outputRow.MinP, outputRow.MaxQ, outputRow.MinQ, outputRow.TotalMin };
                if (string.IsNullOrEmpty(Input.PathOutputFile))
                {
                    Helper.CreateCsvFile(DefaultPathForOutputFile, collumnsName, valArray);
                }
                else
                {
                    Helper.CreateCsvFile(Input.PathOutputFile, collumnsName, valArray);
                }
            }
            else
            {
                Console.WriteLine("Создание файла невозможно");
            }
        }
    }
}
