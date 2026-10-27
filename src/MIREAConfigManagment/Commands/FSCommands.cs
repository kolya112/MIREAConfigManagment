using MIREAConfigManagment.Models;
using System;
using System.Text;

namespace MIREAConfigManagment.Commands
{
    internal static class FSCommands
    {
        /// <summary>
        /// Метод выполнения команды ls
        /// </summary>
        /// <param name="args">аргументы</param>
        internal static void Ls(string[] args)
        {
            // Определение директории для сканирования
            DirectoryModel directoryForScan;

            if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
                directoryForScan = Global.CurrentDirectory;
            else
            {
                var findedDir = VFS.FindDirectory(args[0]);
                if (findedDir == null)
                {
                    Console.WriteLine("Директория по указанному пути не найдена");
                    return;
                }
                else
                    directoryForScan = findedDir;
            }

            // Сканирование директории на поддиректории
            var strBuilder = new StringBuilder("Директории:" + Environment.NewLine);

            foreach (DirectoryModel dir in directoryForScan.Directories)
                strBuilder.Append(dir.Name).Append(' ');

            // Сканирование директории на файлы
            strBuilder.Append(Environment.NewLine).AppendLine("Файлы:");

            foreach (FileModel file in directoryForScan.Files)
                strBuilder.Append(file.Name).Append(' ');

            Console.WriteLine(strBuilder.ToString());
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
