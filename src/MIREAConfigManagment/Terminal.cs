using System;
using System.Text.RegularExpressions;

namespace MIREAConfigManagment
{
    internal class Terminal
    {
        /// <summary>
        /// Метод запуска терминала
        /// </summary>
        internal void Start()
        {
            LogsHandler.InitLogFile();

            while (true)
            {
                Console.Write(Global.Prompt);

                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                    continue;

                input = ResolveEnvVariables(input);

                Commands.CommandsEngine.Parse(input);
            }
        }

        /// <summary>
        /// Метод разрешения переменных окружения
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Измененная строка</returns>
        internal static string ResolveEnvVariables(string input)
        {
            return Regex.Replace(input, "\\$([^\\s]+)", match =>
            {
                return Environment.GetEnvironmentVariable(match.Groups[1].Value) ?? "";
            });
        }
    }
}
