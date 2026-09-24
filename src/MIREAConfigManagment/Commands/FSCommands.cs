using System;

namespace MIREAConfigManagment.Commands
{
    internal class FSCommands
    {
        /// <summary>
        /// Метод выполнения команды ls
        /// </summary>
        /// <param name="args">аргументы</param>
        internal static void Ls(string[] args)
        {
            string output = "ls";
            foreach (string arg in args)
                output += " " + arg;

            Console.WriteLine(output);
        }

        /// <summary>
        /// Метод выполнения команды cd
        /// </summary>
        /// <param name="args">аргументы</param>
        internal static void Cd(string[] args)
        {
            string output = "cd";
            foreach (string arg in args)
                output += " " + arg;

            Console.WriteLine(output);
        }
    }
}
