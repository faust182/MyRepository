using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ParseCSV
{
    public class InputData
    {
        public string PathInputFile { get; set; }

        public string PathOutputFile { get; set; }

        public string Month { get; set; }

        public int Year { get; set; }

        public void GetPathInputFile(IUserInput userInput)
        {
            bool isPathInputFileCorrect = false;

            Console.WriteLine("Введите полный путь до файла с данными (пример: d:\\Program Files\\...\\Example meters.CSV)\n");
            do
            {
                string input = userInput.GetInput();
                if (new FileInfo(input).Exists)
                {
                    PathInputFile = input;
                    isPathInputFileCorrect = true;
                }
                else
                {
                    Console.WriteLine("Неверно указан путь до файла-источника \n" +
                        "Повторите попытку\n");
                }
            }
            while (!isPathInputFileCorrect && !userInput.IsTestMode);
            Console.WriteLine();
        }

        public void GetPathOutputFile(IUserInput userInput)
        {
            bool isPathOutputFileCorrect = false;
            Console.WriteLine();
            Console.WriteLine("Введите полный путь до файла в который будут помещены результаты\n" + 
                @"(пример: d:\Program Files\...\Result.CSV). Если путь не будет указан, то файл ""output.CSV""" + 
                "с результатами будет находиться по директории запуска исполняемого файла\n");

            do
            {
                var pathOutputFile = userInput.GetInput();
                var fileName = Helper.GetFileName(pathOutputFile);
                if (Helper.IsPathOutputFileExist(pathOutputFile) && Helper.IsCsvFileNameCorrect(fileName))
                {
                    PathOutputFile = pathOutputFile;
                    isPathOutputFileCorrect = true;
                }
                else
                {
                    if (Helper.IsCsvFileNameCorrect(fileName) == false)
                    {
                        Console.WriteLine("Неверно указано имя файла (используются недопустимые символы или неверно указано расширение)");
                    }
                    else if (Helper.IsPathOutputFileExist(pathOutputFile) == false)
                    {
                        Console.WriteLine("Неверно указана директория");
                    }

                    Console.WriteLine("Повторите попытку\n");
                }
            }
            while (!isPathOutputFileCorrect && !userInput.IsTestMode);
        }

        public void GetYear(IUserInput userInput)
        {
            bool isYearCorrect = false;
            int year;
            Console.WriteLine();
            Console.WriteLine("Введите год (допускается ввод тоько полного значения):");
            do
            {
                isYearCorrect = int.TryParse(userInput.GetInput(), out year);
                if (isYearCorrect && year > 2000 && year < 2200)
                {
                    Year = year;
                }
                else
                {
                    Console.WriteLine("Неверно указан год \n" +
                        "Повторите попытку\n");
                }
            }
            while (!isYearCorrect && !userInput.IsTestMode);
        }

        public void GetMonth(IUserInput userInput)
        {
            int month;
            Console.WriteLine();
            Console.WriteLine("Введите название месяца (кириллицей) или его порядковый номер:");
            do
            {
                string inputMonth = userInput.GetInput();
                Console.WriteLine();
                month = Helper.GetMonthNumber(inputMonth);
                if (month != 0)
                {
                    Month = inputMonth;
                }
                else
                {
                    Console.WriteLine("Неверно указан месяц \n" +
                        "Повторите попытку\n");
                }
            }
            while (month == 0);
        }
    }
}
