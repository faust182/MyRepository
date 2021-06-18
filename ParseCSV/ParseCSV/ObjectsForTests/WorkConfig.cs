using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    class WorkConfig : IUserInput
    {
        public bool IsTestMode { get; } = false;
        public string GetInput()
        {
            return Console.ReadLine().Trim();
        }
    }
}
