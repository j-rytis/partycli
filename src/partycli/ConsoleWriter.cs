using System;
using partycli.Domain;

namespace partycli
{
    public class ConsoleWriter : IConsoleWriter
    {
        public void Output(string message)
        {
            Console.WriteLine(message);
        }
    }
}
