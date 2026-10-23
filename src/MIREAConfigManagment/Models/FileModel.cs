using System;
using System.Text.Json.Serialization;

namespace MIREAConfigManagment.Models
{
    public class FileModel
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Головная директория
        /// </summary>
        [JsonIgnore]
        public DirectoryModel? HeadDirectory { get; set; }

        /// <summary>
        /// Содержимое файла
        /// </summary>
        public byte[] Content { get; set; } = [];
    }
}
