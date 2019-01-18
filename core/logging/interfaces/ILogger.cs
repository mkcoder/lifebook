namespace lifebook.core.logging
{
    public interface ILogger
    {
        void Error(string message);
        void Information(string message);
        void Verbose(string message);
    }
}