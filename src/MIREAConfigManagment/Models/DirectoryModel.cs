using System;

namespace MIREAConfigManagment.Models
{
    public class DirectoryModel
    {
        public string Name { get; set; } = "/";
        public List<DirectoryModel> Directories { get; set; } = new();
        public List<FileModel> Files { get; set; } = new();
    }
}
