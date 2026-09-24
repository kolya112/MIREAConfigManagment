using System;

namespace MIREAConfigManagment.Commands
{
    internal class CommandsEngine
    {
        /// <summary>
        /// Метод запуска командного движка: парсит входную строку
        /// </summary>
        /// <param name="input">входная строка</param>
        internal static void Parse(string input)
        {
            string[] commandSplit = input.Split(' ');
            string command = commandSplit[0];
            string[] args = commandSplit.Skip(1).ToArray();

            Execute(command, args);
        }

        /// <summary>
        /// Метод определения команды и запуск выполнения при обнаружении
        /// </summary>
        /// <param name="command">команда</param>
        /// <param name="args">аргументы</param>
        private static void Execute(string command, string[] args)
        {
            switch (command)
            {
                case "ls":
                    FSCommands.Ls(args);
                    break;
                case "cd":
                    FSCommands.Cd(args);
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Command not found");
                    break;
            }
        }
    }
}
