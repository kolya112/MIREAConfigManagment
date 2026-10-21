using System;

namespace MIREAConfigManagment.Models
{
    public class LogModel
    {
        /// <summary>
        /// Имя команды
        /// </summary>
        public string Command { get; set; } = "";

        /// <summary>
        /// Массив параметров при наличии
        /// </summary>
        public string[]? Arguments { get; set; }

        /// <summary>
        /// Дата и время события
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
