using System.Text;

namespace MIREAConfigManagment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "REPL";

            // Обработка аргументов, если они переданы
            // 1) VFS; 2) Prompt; 3) Log-file; 4) Startup-file
            if (args.Length > 4)
                throw new ArgumentOutOfRangeException(nameof(args), "Ожидалось не более 4 параметров");

            if (args.Length > 0)
            {
                var paramsOutputBuilder = new StringBuilder("Parameters:");

                for (int i = 0; i < args.Length; i++)
                {
                    string param = args[i];
                    paramsOutputBuilder.Append(" " + param);

                    switch (i)
                    {
                        case 0: // VFS
                            Global.VFSPath = param;
                            break;
                        case 1: // Prompt
                            Global.Prompt = param;
                            break;
                        case 2: // Log-file
                            Global.LogPath = param;
                            break;
                        case 3: // Startup-file
                            if (!File.Exists(param))
                                throw new FileNotFoundException("Указанный стартовый скрипт не найден");

                            Global.StartupPath = param;
                            break;
                    }
                }

                Console.WriteLine(paramsOutputBuilder.ToString()); // Debug вывод всех параметров
            }

            var terminal = new Terminal();
            terminal.Start();
        }
    }
}
