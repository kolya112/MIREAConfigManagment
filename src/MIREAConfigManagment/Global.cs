using System;
using MIREAConfigManagment.Models;

namespace MIREAConfigManagment
{
    internal static class Global
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        internal static string Username { get; set; } = Environment.UserName;

        /// <summary>
        /// Имя хоста
        /// </summary>
        internal static string Hostname { get; set; } = Environment.MachineName;

        /// <summary>
        /// Путь к VFS
        /// </summary>
        internal static string VFSPath { get; set; } = "main.vfs";

        /// <summary>
        /// Приглашение к вводу
        /// </summary>
        internal static string Prompt { get; set; } = $"{Username}@{Hostname}:~$ ";

        /// <summary>
        /// Путь к лог-файлу
        /// </summary>
        internal static string LogPath { get; set; } = "actions.log";

        /// <summary>
        /// Путь к стартовому скрипту
        /// </summary>
        internal static string? StartupPath { get; set; }

        /// <summary>
        /// Актуальный лог событий терминала
        /// </summary>
        internal static List<LogModel> Logs { get; set; } = new();

        /// <summary>
        /// Корневая директория
        /// </summary>
        internal static DirectoryModel RootDirectory { get; set; } = new() { Name = "Root" };

        /// <summary>
        /// Текущая директория
        /// </summary>
        internal static DirectoryModel CurrentDirectory { get; set; } = RootDirectory;
    }
}
