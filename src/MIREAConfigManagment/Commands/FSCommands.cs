using System;
using MIREAConfigManagment.Models;
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
            if (!(args.Length < CommandsEngine.TWO))
                return;

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

            var strBuilder = new StringBuilder("Директории:" + Environment.NewLine);

            foreach (DirectoryModel dir in directoryForScan.Directories)
                strBuilder.Append(dir.Name).Append(' ');

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
            if (args.Length != 1 || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Указан некорректный путь к директории");
                return;
            }

            DirectoryModel? foundedDir = VFS.FindDirectory(args[0]);
            if (foundedDir != null)
                Global.CurrentDirectory = foundedDir;
            else
                Console.WriteLine("Директории по указанному пути не существует");
        }

        /// <summary>
        /// Метод выполнения команды du
        /// </summary>
        /// <param name="args">аргументы</param>
        internal static void Du(string[] args)
        {
            if (!(args.Length < CommandsEngine.TWO))
                return;

            object? vfsObject;

            if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
                vfsObject = Global.CurrentDirectory;
            else
                vfsObject = VFS.FindEntry(args[0]);

            if (vfsObject is DirectoryModel dir)
            {
                var filesInDir = VFS.FindFilesRecursive(dir);
                var strBuilder = new StringBuilder($"Размер файлов в директории {dir.Name}:{Environment.NewLine}");
                long totalBytes = 0;
                int index = 0;
                foreach (FileModel file in filesInDir)
                {
                    index++;
                    totalBytes += file.Content.LongLength;
                    strBuilder.Append(index + " " + file.Name + " ").AppendLine(file.Content.LongLength.ToString());
                }
                strBuilder.Append($"Всего файлов: {index} | Общий размер: {totalBytes} байт");
                Console.WriteLine(strBuilder.ToString());
            }
            else if (vfsObject is FileModel file)
                Console.WriteLine($"Размер файла {file.Name}: {file.Content.LongLength} байт");
            else
                Console.WriteLine("Директория или файл по указанному пути не найдена");
        }

        /// <summary>
        /// Метод выполнения команды mv
        /// </summary>
        /// <param name="args">аргументы</param>
        internal static void Mv(string[] args)
        {
            if (args.Length != CommandsEngine.TWO || string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]))
            { Console.WriteLine("Переданы некорректные аргументы: mv <откуда> <куда>"); return; }

            var moveResult = VFS.MoveEntry(args[0], args[1]);
            switch (moveResult)
            {
                case VFS.MoveResult.Success:
                    Console.WriteLine("Перемещение выполнено");
                    break;
                case VFS.MoveResult.PathNullOrEmpty:
                    Console.WriteLine("Передан некорректный путь");
                    break;
                case VFS.MoveResult.NotFound:
                    Console.WriteLine("Директория либо файл по указанному пути не найден (-а)");
                    break;
                case VFS.MoveResult.Dublicate:
                    Console.WriteLine("При перемещении был обнаружен объект-дубликат. Перемещение отменено.");
                    break;
                case VFS.MoveResult.Restricted:
                    Console.WriteLine("Ошибка доступа к объекту при выполнении перемещения");
                    break;
            }
        }
    }
}
