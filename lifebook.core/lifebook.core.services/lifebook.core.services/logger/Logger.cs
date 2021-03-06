﻿using System;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using lifebook.core.logging.interfaces;
using Serilog.Sinks.SystemConsole.Themes;

namespace lifebook.core.logging.services
{
    public sealed class Logger : interfaces.ILogger
    {

        private Serilog.Core.Logger _logger;

        public Logger()
        {
            _logger = new LoggerConfiguration()
                            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                            {
                                AutoRegisterTemplate = true,
                                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
                            })
                            .WriteTo.File("./logs", Serilog.Events.LogEventLevel.Error)
                            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                            .CreateLogger();
        }

        public void Error(Exception ex, string message) => _logger
                                                            .ForContext(GetType())
                                                            .Error(ex, message);

        public void Error(string message) => _logger.Error(message);

        public void Error(Exception ex, string message, params object[] o)
        {
            _logger.Error(ex, message, o);
        }

        public void Information(string message) => _logger.Information(message);

        public void Information(string message, params object[] o)
        {
            _logger.Information(message, o);
        }

        public void Verbose(string message) => _logger.Verbose(message);

        public void Verbose(string message, params object[] o)
        {
            _logger.Verbose(message, o);
        }
    }
}