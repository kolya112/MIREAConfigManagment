using System;

namespace MIREAConfigManagment.Commands
{
    internal class FSCommands
    {
        internal static void Ls(string[] args)
        {
            string output = "ls";
            foreach (string arg in args)
                output += " " + arg;

            Console.WriteLine(output);
        }

        internal static void Cd(string[] args)
        {
            string output = "cd";
            foreach (string arg in args)
                output += " " + arg;

            Console.WriteLine(output);
        }
    }
}
