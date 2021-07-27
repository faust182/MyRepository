using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ParseCSV
{
    class PathOutputFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {    
            string pathOutputFile = value.ToString();
            var fileName = Helper.GetFileName(pathOutputFile);
            if ((Helper.IsPathOutputFileExist(pathOutputFile) && Helper.IsCsvFileNameCorrect(fileName)) || string.IsNullOrEmpty(pathOutputFile))
            {
                return true;
            }
            else
            {
                this.ErrorMessage = "Неверно указано имя файла с результатами или директория не существует";
                return false;
            }
        }
    }
}
