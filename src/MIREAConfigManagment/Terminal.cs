using System;

namespace MIREAConfigManagment
{
    internal class Terminal
    {
        public Terminal()
        {
            Global.Username = Environment.UserName;
            Global.Hostname = Environment.MachineName;
        }

        internal void Start()
        {
            while (true)
            {
                Console.Write($"{Global.Username}@{Global.Hostname}:~$ ");

                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                    continue;


            }    
        }
    }
}
