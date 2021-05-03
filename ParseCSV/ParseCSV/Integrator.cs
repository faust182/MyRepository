using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ParseCSV
{
    class Integrator
    {
        static int m = 0;
        static int y = 0;
        public int ratio { get; set; } = 12000;
        static string path_inp_f;
        public string path_out_f = Directory.GetCurrentDirectory() + "\\output.CSV";
        public Integrator(string path, int month, int year) { path_inp_f = path; m = month; y = year; } // конструктор
        static string[,] array = Support.GetArrayFromCsvFile(path_inp_f);
        static (int, int) range = Support.GetRowsRangeByMonth(array, m, y);

        public double SumRowsP { get; } = 0;
        public double SumRowsQ { get; } = 0;
        public double Psq { get; } = 0;
        public double Qsq { get; } = 0;
        public double MaxP { get; } = 0;
        public double MinP { get; } = 0;
        public double MaxQ { get; } = 0;
        public double MinQ { get; } = 0;
        public Integrator()
        {
            for (int i = 0; i < array.GetLength(1) - 1; i++)
            {

            }
        }



    }
}
