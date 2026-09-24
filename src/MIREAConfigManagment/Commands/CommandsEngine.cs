using System;

namespace MIREAConfigManagment.Commands
{
    internal class CommandsEngine
    {
        internal static void Parse(string input)
        {
            string[] commandSplit = input.Split(' ');
            string command = commandSplit[0];
            string[] args = commandSplit.Skip(1).ToArray();

            Execute(command, args);
        }

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
                default:
                    Console.WriteLine("Command not found");
                    break;
            }
        }
    }
}
