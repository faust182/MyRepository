using System.ComponentModel.DataAnnotations;
using System.IO;


namespace ParseCSV
{
    class PathOutputFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {   
            if (value != null)
            {
                string pathOutputFile = value.ToString();
                var fileName = Helper.GetFileName(pathOutputFile);
                if (Helper.IsPathOutputFileExist(pathOutputFile) && Helper.IsCsvFileNameCorrect(fileName))
                    return true;
                else
                    this.ErrorMessage = "Неверно указано имя файла или директория";
            }
            return false;
        }
    }
}
