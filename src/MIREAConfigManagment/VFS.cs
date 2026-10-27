using System;
using System.Text.Json;
using MIREAConfigManagment.Models;

namespace MIREAConfigManagment
{
    internal class VFS
    {
        internal enum MoveResult
        {
            Success,
            PathNullOrEmpty,
            NotFound,
            Dublicate,
            Restricted
        }

        /// <summary>
        /// Инициализация файла VFS
        /// </summary>
        internal static void InitVFSFile()
        {
            if (File.Exists(Global.VFSPath))
            {
                try
                {
                    DirectoryModel? rootDir = JsonSerializer.Deserialize<DirectoryModel>(File.ReadAllText(Global.VFSPath));
                    if (rootDir != null)
                    {
                        Global.RootDirectory = rootDir;
                        RestoreHeads();
                    }
                    else
                        Console.WriteLine("Некорректный формат VFS в предоставленном файле VFS");
                }
                catch (JsonException)
                {
                    Console.WriteLine("Некорректный формат JSON в файле VFS");
                }
            }
            else
                Console.WriteLine("Указанный файл VFS не найден");
        }

        /// <summary>
        /// Поиск файла
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns>Модель файла FileModel, иначе null</returns>
        internal static FileModel? FindFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (path.EndsWith('/'))
                return null;

            int lastSlashPosition = path.LastIndexOf('/');
            DirectoryModel? dir;
            string fileName;
            if (lastSlashPosition == -1)
                { dir = Global.CurrentDirectory; fileName = path; }
            else if (lastSlashPosition == 0)
                { dir = Global.RootDirectory; fileName = path.Substring(1); }
            else
            {
                string dirPath = path.Substring(0, lastSlashPosition);
                fileName = path.Substring(lastSlashPosition + 1);

                dir = FindDirectory(dirPath);
                if (dir == null)
                    return null;
            }

            foreach (FileModel file in dir.Files)
            {
                if (file.Name == fileName)
                    return file;
            }

            return null;
        }

        /// <summary>
        /// Поиск директории
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="current">Директория, от которой ведется поиск</param>
        /// <returns>Модель директории DirectoryModel, иначе null</returns>
        internal static DirectoryModel? FindDirectory(string path, DirectoryModel? current = null)
        {
            if (string.IsNullOrEmpty(path)) 
                return null;

            if (current == null) 
                current = Global.CurrentDirectory;

            while (path.Contains("//")) 
                path = path.Replace("//", "/");

            if (path == "/") 
                return Global.RootDirectory;
            else if (path == ".") 
                return current;
            else if (path == "..") 
                return current.HeadDirectory ?? current;

            if (!path.Contains('/')) 
                return FindLocalDirectory(path, current);

            string[] parts = path.Split('/', 2);

            if (parts[0] == "" && parts[1] != "")
                return FindDirectory(parts[1], Global.RootDirectory);
            else if (parts[0] != "" && parts[1] == "")
                return FindDirectory(parts[0], current);
            else if (parts[0] == "." && parts[1] != "")
                return FindDirectory(parts[1], current);
            else if (parts[0] == ".." && parts[1] != "")
                return FindDirectory(parts[1], current.HeadDirectory ?? current);
            else if (parts[0] != "" && parts[1] != "")
            {
                var findedDirectory = FindLocalDirectory(parts[0], current);
                if (findedDirectory != null)
                    return FindDirectory(parts[1], findedDirectory);
            }

            return null;
        }

        /// <summary>
        /// Поиск директории или файла
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns>Модель директории DirectoryModel, модель файла FileModel либо null</returns>
        internal static object? FindEntry(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            DirectoryModel? dir = FindDirectory(path);
            if (dir != null)
                return dir;

            return FindFile(path);
        }

        /// <summary>
        /// Перемещение директории или файла
        /// </summary>
        /// <param name="fromPath">Исходный путь</param>
        /// <param name="toPath">Путь назначения</param>
        /// <returns>Результат перемещения MoveResult</returns>
        internal static MoveResult MoveEntry(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath) || string.IsNullOrEmpty(toPath))
                return MoveResult.PathNullOrEmpty;

            object? source = FindEntry(fromPath);

            if (source is DirectoryModel dir)
                return MoveDirectory(dir, toPath);

            if (source is FileModel file)
                return MoveFile(file, toPath);

            return MoveResult.NotFound;
        }

        private static MoveResult MoveDirectory(DirectoryModel dir, string toPath)
        {
            if (dir == Global.RootDirectory)
                return MoveResult.Restricted;

            DirectoryModel? target = FindMoveTargetDirectory(
                toPath, dir.Name, out string newName);

            if (target == null)
                return MoveResult.NotFound;

            if (dir == target || CheckDirectoryParent(target, dir))
                return MoveResult.Restricted;

            if (FindLocalDirectory(newName, target) != null ||
                FindLocalFile(newName, target) != null)
                return MoveResult.Dublicate;

            dir.HeadDirectory?.Directories.Remove(dir);
            dir.HeadDirectory = target;
            dir.Name = newName;
            target.Directories.Add(dir);

            return MoveResult.Success;
        }

        private static MoveResult MoveFile(FileModel file, string toPath)
        {
            DirectoryModel? target = FindMoveTargetDirectory(
                toPath, file.Name, out string newName);

            if (target == null)
                return MoveResult.NotFound;

            if (FindLocalDirectory(newName, target) != null ||
                FindLocalFile(newName, target) != null)
                return MoveResult.Dublicate;

            file.HeadDirectory?.Files.Remove(file);
            file.HeadDirectory = target;
            file.Name = newName;
            target.Files.Add(file);

            return MoveResult.Success;
        }

        private static DirectoryModel? FindMoveTargetDirectory(
            string toPath, string originalName, out string newName)
        {
            DirectoryModel? target = FindDirectory(toPath);
            newName = originalName;

            if (target != null)
                return target;

            int lastSlashIndex = toPath.LastIndexOf('/');
            newName = toPath.Substring(lastSlashIndex + 1);

            if (string.IsNullOrEmpty(newName))
                return null;

            if (lastSlashIndex == -1)
                return Global.CurrentDirectory;

            if (lastSlashIndex == 0)
                return Global.RootDirectory;

            return FindDirectory(toPath.Substring(0, lastSlashIndex));
        }

        /// <summary>
        /// Поиск директории внутри одной директории
        /// </summary>
        /// <param name="name">Название директории</param>
        /// <param name="current">Директория, в которой ведется поиск</param>
        /// <returns>Модель директории DirectoryModel, иначе null</returns>
        private static DirectoryModel? FindLocalDirectory(string name, DirectoryModel current)
        {
            foreach (DirectoryModel dir in current.Directories)
            {
                if (dir.Name == name) 
                    return dir;
            }

            return null;
        }

        /// <summary>
        /// Поиск файла внутри одной директории
        /// </summary>
        /// <param name="name">Имя файла</param>
        /// <param name="current">Директория для поиска</param>
        /// <returns>Если найдено - модель файла FileModel, иначе null</returns>
        private static FileModel? FindLocalFile(string name, DirectoryModel current)
        {
            foreach (FileModel file in current.Files)
            {
                if (file.Name == name) 
                    return file;
            }

            return null;
        }

        /// <summary>
        /// Проверить вхождение директории target в иерархии директории source
        /// </summary>
        /// <param name="source">Целевая директория</param>
        /// <param name="target">Искомая директория</param>
        /// <returns>Если найдено - true, иначе false</returns>
        private static bool CheckDirectoryParent(DirectoryModel source, DirectoryModel target)
        {
            DirectoryModel currentDir = source;

            while (currentDir.HeadDirectory != null)
            {
                if (currentDir.HeadDirectory == target)
                    return true;

                currentDir = currentDir.HeadDirectory;
            }

            return false;
        }

        /// <summary>
        /// Восстановление ссылок на головные директории для директорий и файлов
        /// </summary>
        private static void RestoreHeads()
        {
            Stack<DirectoryModel> dirsToRehead = new();

            Global.RootDirectory.HeadDirectory = null;

            dirsToRehead.Push(Global.RootDirectory);

            while (dirsToRehead.Count > 0)
            {
                var lastDir = dirsToRehead.Pop();

                foreach (var dir in lastDir.Directories)
                {
                    dir.HeadDirectory = lastDir;
                    dirsToRehead.Push(dir);
                }

                foreach (var file in lastDir.Files)
                {
                    file.HeadDirectory = lastDir;
                }
            }
        }
    }
}
