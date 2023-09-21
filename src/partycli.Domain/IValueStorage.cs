namespace partycli.Domain
{
    public interface IValueStorage
    {
        void Store(string name, string value, bool writeToConsole);
    }
}
