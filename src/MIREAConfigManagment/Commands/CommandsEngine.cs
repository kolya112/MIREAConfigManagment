using System;

namespace MIREAConfigManagment.Commands
{
    internal static class CommandsEngine
    {
        internal const int TWO = 2;

        /// <summary>
        /// Метод запуска командного движка: парсит входную строку
        /// </summary>
        /// <param name="input">входная строка</param>
        internal static void Parse(string input)
        {
            string[] commandSplit = input.Split(' ', 
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
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
            Global.CommandsHistory.Add($"{command} {string.Join(' ', args)}");
            switch (command)
            {
                case "ls":
                    if (args.Length < TWO)
                    {
                        LogsHandler.PushLog(command, args);
                        FSCommands.Ls(args);
                    }
                    break;
                case "cd":
                    if (args.Length == 1)
                    {
                        LogsHandler.PushLog(command, args);
                        FSCommands.Cd(args);
                    }
                    break;
                case "du":
                    if (args.Length < TWO)
                    {
                        LogsHandler.PushLog(command, args);
                        FSCommands.Du(args);
                    }
                    break;
                case "exit":
                    LogsHandler.PushLog(command, args);
                    Environment.Exit(0);
                    break;
                case "echo":
                    if (args.Length > 0)
                    {
                        LogsHandler.PushLog(command, args);
                        Console.WriteLine(string.Join(' ', args));
                    }
                    break;
                case "regvar":
                    if (args.Length == TWO)
                    {
                        LogsHandler.PushLog(command, args);
                        Environment.SetEnvironmentVariable(args[0], args[1]);
                    }
                    break;
                default:
                    Console.WriteLine("Command not found");
                    break;
            }
        }
    }
}
