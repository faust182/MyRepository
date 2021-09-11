using System;

namespace ParseCSV.DAL
{
    class Counter
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// серийный номер счетчика
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// модель
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// место устновки
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// коэффициент преобразования по току
        /// </summary>
        public int RatioI { get; set; }

        /// <summary>
        /// коэффициент преобразования по напряжению
        /// </summary>
        public int RatioU { get; set; }

        /// <summary>
        /// дата прохождения поверки
        /// </summary>
        public DateTime CurrentVerification { get; set; }

        /// <summary>
        /// дата следующей поверки
        /// </summary>
        public DateTime NextVerification { get; set; }
    }   
}
