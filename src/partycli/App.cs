using partycli.Enums;
using System;

namespace partycli
{
    public class App
    {
        private readonly IArgumentProcessor argumentProcessor;

        public App(IArgumentProcessor argumentProcessor)
        {
            this.argumentProcessor = argumentProcessor;
        }

        public void Run(string[] args)
        {
            var currentState = argumentProcessor.ProcessArguments(args);

            if (currentState == State.none)
            {
                Console.WriteLine("To get and save all servers, use command: partycli.exe server_list");
                Console.WriteLine("To get and save France servers, use command: partycli.exe server_list --france");
                Console.WriteLine("To get and save servers that support TCP protocol, use command: partycli.exe server_list --TCP");
                Console.WriteLine("To see saved list of servers, use command: partycli.exe server_list --local ");
            }
            Console.Read();
        }
    }
}
