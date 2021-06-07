using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    class InputData
    {
        public string PathInputFile { get; set; }

        public string PathOutputFile { get; set; }

        public string Month { get; set; }

        public int Year { get; set; }

        public void GetPathInputFile()
        {
            bool isPathInputFileCorrect = false;
            Console.WriteLine(@"Введите полный путь до файла с данными (пример: d:\Program Files\...\Example meters.CSV)");
            Console.WriteLine();
            do
            {
                string input = Console.ReadLine();
                if (Validator.IsPathInputFileCorret(input))
                {
                    PathInputFile = input;
                    isPathInputFileCorrect = true;
                }
                else
                {
                    Console.WriteLine("Неверно указан путь до файла-источника");
                    Console.WriteLine("Повторите попытку");
                    Console.WriteLine();
                }
            }
            while (!isPathInputFileCorrect);
            Console.WriteLine();
        }

        public void GetPathOutputFile()
        {
            bool isPathOutputFileCorrect = false;
            Console.WriteLine();
            Console.WriteLine(@"Введите полный путь до файла в который будут помещены результаты (пример: d:\Program Files\...\Result.CSV). Если путь не будет указан, то файл ""output.CSV"" с результатами будет находиться по директории запуска исполняемого файла");
            Console.WriteLine();
            do
            {
                var pathOutputFile = Console.ReadLine();
                var validation = Validator.IsPathOutputFileCorret(pathOutputFile);
                if (validation.isNameCorrect && validation.isPathCorrect)
                {
                    PathOutputFile = pathOutputFile;
                    isPathOutputFileCorrect = true;
                }
                else
                {
                    if (validation.isNameCorrect == false)
                    {
                        Console.WriteLine("Неверно указано имя файла (используются недопустимые символы или неверно указано расширение)");
                    }

                    if (validation.isPathCorrect == false)
                    {
                        Console.WriteLine("Неверно указана директория");
                    }

                    Console.WriteLine("Повторите попытку");
                    Console.WriteLine();
                }
            }
            while (!isPathOutputFileCorrect);
        }

        public void GetYear()
        {
            bool isYearCorrect = false;
            int year;
            Console.WriteLine();
            Console.WriteLine("Введите год (допускается ввод тоько полного значения):");
            do
            {
                isYearCorrect = int.TryParse(Console.ReadLine(), out year);
                if (isYearCorrect)
                {
                    Year = year;
                }
                else
                {
                    Console.WriteLine("Неверно указан год");
                    Console.WriteLine("Повторите попытку");
                    Console.WriteLine();
                }
            }
            while (!isYearCorrect);
        }

        public void GetMonth()
        {
            int month;
            Console.WriteLine();
            Console.WriteLine("Введите название месяца (кириллицей) или его порядковый номер:");
            do
            {
                string inputMonth = Console.ReadLine();
                Console.WriteLine();
                month = Validator.GetMonthNumber(inputMonth);
                if (month != 0)
                {
                    Month = inputMonth;
                }
                else
                {
                    Console.WriteLine("Неверно указан месяц");
                    Console.WriteLine("Повторите попытку");
                    Console.WriteLine();
                }
            }
            while (month == 0);
        }
    }
}
