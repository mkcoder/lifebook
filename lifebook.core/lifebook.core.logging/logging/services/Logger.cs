﻿using System;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using lifebook.core.logging.interfaces;

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
                            }).CreateLogger();
        }

        public void Error(string message) => _logger.Error(message);
        public void Information(string message) => _logger.Information(message);
        public void Verbose(string message) => _logger.Verbose(message);        
    }
}