using System.ComponentModel.DataAnnotations;
using System.IO;


namespace ParseCSV
{
    class PathInputFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string pathInputFile = value.ToString();
                if (new FileInfo(pathInputFile).Exists)
                    return true;
                else
                    this.ErrorMessage = "Указанный файл не существует";
            }
            return false;
        }
    }
}
