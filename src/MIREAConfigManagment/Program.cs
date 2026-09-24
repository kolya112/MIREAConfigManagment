namespace MIREAConfigManagment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "REPL";
            var terminal = new Terminal();
            terminal.Start();
        }
    }
}
