using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ParseCSV
{
    static class Validator
    {
        static StringBuilder path = new StringBuilder();

        public static bool IsPathInputFileCorret(string inputPath)
        {
            path.Clear();
            var pathArray = inputPath.Split('\\');
            for (int i = 0; i < pathArray.Length - 1; i++)
            {
                path.Append(pathArray[i] + "\\");
            }

            return (Directory.Exists(path.ToString()) && new FileInfo(inputPath).Exists) ? true : false;
        }

        public static (bool isNameCorrect, bool isPathCorrect) IsPathOutputFileCorret(string inputPath)
        {
            var output = (isNameCorrect: false, isPathCorrect: false);
            path.Clear();
            var pathArray = inputPath.Split('\\');
            var outputFileName = pathArray[pathArray.Length - 1];
            for (int i = 0; i < pathArray.Length - 1; i++)
            {
                path.Append(pathArray[i] + "\\");
            }

            output.isNameCorrect = IsCsvFileNameCorrect(outputFileName);
            output.isPathCorrect = Directory.Exists(path.ToString());
            return output;
        }

        public static int GetMonthNumber(string month)
        {
            switch (month.ToLower())
            {
                case "1":
                case "январь":
                    return 1;
                case "2":
                case "февраль":
                    return 2;
                case "3":
                case "март":
                    return 3;
                case "4":
                case "апрель":
                    return 4;
                case "5":
                case "май":
                    return 5;
                case "6":
                case "июнь":
                    return 6;
                case "7":
                case "июль":
                    return 7;
                case "8":
                case "август":
                    return 8;
                case "9":
                case "сентябрь":
                    return 9;
                case "10":
                case "октябрь":
                    return 10;
                case "11":
                case "ноябрь":
                    return 11;
                case "12":
                case "декабрь":
                    return 12;
                default: return 0;
            }
        }

        static bool IsCsvFileNameCorrect(string fileName)
        {
            bool isExpansionCorrect = true;
            bool areCharsCorrect = true;
            var fileNameArray = fileName.Split('.');
            foreach (var currentChar in fileName)
            {
                foreach (var item in Path.GetInvalidFileNameChars())
                {
                    if (currentChar == item)
                    {
                        areCharsCorrect = false;
                    }
                }
            }

            if (fileNameArray[fileNameArray.Length - 1].ToLower() != "csv")
            {
                isExpansionCorrect = false;
            }

            return areCharsCorrect && isExpansionCorrect ? true : false;
        }
    }
}
