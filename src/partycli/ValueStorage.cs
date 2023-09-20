using System;

namespace partycli.Domain
{
    public interface IValueStorage
    {
        void Store(string name, string value, bool writeToConsole);
    }

    public class ValueStorage : IValueStorage
    {
        public void Store(string name, string value, bool writeToConsole = true)
        {
            try
            {
                var settings = Properties.Settings.Default;
                settings[name] = value;
                settings.Save();
                if (writeToConsole)
                {
                    Console.WriteLine("Changed " + name + " to " + value);
                }
            }
            catch
            {
                Console.WriteLine("Error: Couldn't save " + name + ". Check if command was input correctly.");
            }
        }
    }
}
