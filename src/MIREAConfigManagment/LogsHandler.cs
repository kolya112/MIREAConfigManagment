using System;
using System.Text.Json;
using MIREAConfigManagment.Models;

namespace MIREAConfigManagment
{
    internal class LogsHandler
    {
        /// <summary>
        /// Инициализация файла логов терминала
        /// </summary>
        internal static void InitLogFile()
        {
            if (File.Exists(Global.LogPath))
            {
                try
                {
                    Global.Logs = JsonSerializer.Deserialize<List<LogModel>>(File.ReadAllText(Global.LogPath)) ?? new();
                }
                catch (JsonException)
                {
                    Console.WriteLine("Ошибка при обработке лога");
                }
            }
        }

        /// <summary>
        /// Добавление нового события в логи
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        internal static void PushLog(string command, string[] args)
        {
            Global.Logs.Add(new LogModel
            {
                Command = command,
                Arguments = args,
                Timestamp = DateTime.Now
            });

            string json = JsonSerializer.Serialize(Global.Logs);
            File.WriteAllText(Global.LogPath, json);
        }
    }
}
