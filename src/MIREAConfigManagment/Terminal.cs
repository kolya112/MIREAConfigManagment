using System;
using System.Text.RegularExpressions;

namespace MIREAConfigManagment
{
    internal class Terminal
    {
        public Terminal()
        {
            Global.Username = Environment.UserName;
            Global.Hostname = Environment.MachineName;
        }

        /// <summary>
        /// Метод запуска терминала
        /// </summary>
        internal void Start()
        {
            while (true)
            {
                Console.Write($"{Global.Username}@{Global.Hostname}:~$ ");

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
        private static string ResolveEnvVariables(string input)
        {
            return Regex.Replace(input, "\\$.*?(\\s|$)", match =>
            {
                return Environment.GetEnvironmentVariable(match.Value) ?? "";
            });
        }
    }
}
