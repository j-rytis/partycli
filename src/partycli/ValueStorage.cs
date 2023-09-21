using partycli.Domain;

namespace partycli
{
    public class ValueStorage : IValueStorage
    {
        private readonly IConsoleWriter _consoleWriter;

        public ValueStorage(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        public void Store(string name, string value, bool writeToConsole = true)
        {
            try
            {
                var settings = Properties.Settings.Default;
                settings[name] = value;
                settings.Save();
                if (writeToConsole)
                {
                    _consoleWriter.Output("Changed " + name + " to " + value);
                }
            }
            catch
            {
                _consoleWriter.Output("Error: Couldn't save " + name + ". Check if command was input correctly.");
            }
        }
    }
}
