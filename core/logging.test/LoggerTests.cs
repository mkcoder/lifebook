using System;
using lifebook.core.logging;
using lifebook.core.logging.services;
using Xunit;

namespace lifebook.core.logging.test
{
    public class UnitTest1
    {
        Logger _sut = new Logger();

        [Fact]
        public void Logging_Error_Logs_Error()
        {
            _sut.Error("Hello world");
            _sut.Information("Hello world");
            _sut.Verbose("Hello world");
        }
    }
}
