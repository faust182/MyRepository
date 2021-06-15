using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    class ValidationResult
    {
        public ValidationResult(string path)
        {
            Path = path;
        }

        public bool Result { get; set; }

        public string Message { get; set; }

        public string Path { get; set; }

        bool isNameCorrect;
        bool isPathCorrect;


    }
}
