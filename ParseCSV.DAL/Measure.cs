using System;

namespace ParseCSV.DAL
{
    class Measure
    {
        /// <summary>
        /// ID счетчика
        /// </summary>
        public int CounterId { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Время начала периода измерения
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Время окончания периода измерения
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Количество потребленной активной электроэнергии за период
        /// </summary>
        public double ImportP { get; set; }

        /// <summary>
        /// Количество выработанной активной электроэнергии за период
        /// </summary>
        public double ExportP { get; set; }

        /// <summary>
        /// Количество потребленной реактивной электроэнергии за период
        /// </summary>
        public double ImportQ { get; set; }

        /// <summary>
        /// Количество выработанной реактивной электроэнергии за период
        /// </summary>
        public double ExportQ { get; set; }

        /// <summary>
        /// Флаги
        /// </summary>
        public string Flags { get; set; }

    }
}
