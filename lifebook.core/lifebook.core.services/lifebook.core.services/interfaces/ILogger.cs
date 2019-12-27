using System;

namespace lifebook.core.logging.interfaces
{
    public interface ILogger
    {
        void Error(Exception ex, string message);
        void Error(string message);
        void Error(Exception ex, string message, params object[] o);
        void Information(string message);
        void Information(string message, params object[] o);
        void Verbose(string message);
        void Verbose(string message, params object[] o);
    }
}