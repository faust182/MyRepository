using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace ParseCSV
{
    public class InputData
    {
        [Required]
        [PathInputFile]
        public string PathInputFile { get; set; }

        [PathOutputFile]
        public string PathOutputFile { get; set; }

        [Required(ErrorMessage = "Неверно указан месяц")]
        [Range(1, 12)]
        public string Month { get; set; }

        [Required]
        [Range(2000, 2200)]
        public int Year { get; set; }

        public void GetPathInputFile()
        {
            bool isPathInputFileCorrect = false;

            Console.WriteLine("Введите полный путь до файла с данными (пример: d:\\Program Files\\...\\Example meters.CSV)\n");
            do
            {
                string input = Console.ReadLine();
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
            while (!isPathInputFileCorrect);
            Console.WriteLine();
        }

        public void GetPathOutputFile()
        {
            bool isPathOutputFileCorrect = false;
            Console.WriteLine();
            Console.WriteLine("Введите полный путь до файла в который будут помещены результаты\n" + 
                @"(пример: d:\Program Files\...\Result.CSV). Если путь не будет указан, то файл ""output.CSV""" + 
                "с результатами будет находиться по директории запуска исполняемого файла\n");

            do
            {
                var pathOutputFile = Console.ReadLine();
                var fileName = Helper.GetFileName(pathOutputFile);
                if ((Helper.IsPathOutputFileExist(pathOutputFile) && Helper.IsCsvFileNameCorrect(fileName)) || string.IsNullOrEmpty(pathOutputFile))
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
                if (isYearCorrect && year > 2000 && year < 2200)
                {
                    Year = year;
                    isYearCorrect = true;
                }
                else
                {
                    isYearCorrect = false;
                    Console.WriteLine("Неверно указан год \n" +
                        "Повторите попытку\n");
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
