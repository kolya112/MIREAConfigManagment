using System;
using System.Text.Json.Serialization;

namespace MIREAConfigManagment.Models
{
    public class DirectoryModel
    {
        /// <summary>
        /// Название директории
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Головная директория
        /// </summary>
        [JsonIgnore]
        public DirectoryModel? HeadDirectory { get; set; }

        /// <summary>
        /// Список директорий
        /// </summary>
        public List<DirectoryModel> Directories { get; set; } = new();

        /// <summary>
        /// Список файлов
        /// </summary>
        public List<FileModel> Files { get; set; } = new();
    }
}
