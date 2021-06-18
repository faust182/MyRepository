using System;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    public interface IUserInput
    {
        bool IsTestMode { get; }
        string GetInput();
    }
}
