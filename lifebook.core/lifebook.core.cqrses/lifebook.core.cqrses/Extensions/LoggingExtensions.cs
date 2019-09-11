using System;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using lifebook.core.logging.interfaces;
using lifebook.core.logging.services;
using Newtonsoft.Json.Linq;

namespace lifebook.core.cqrses.Extensions
{
    public static class LoggingExtensions
    {
        public static void LogCommand(this ILogger logger, Command command)
        {
            logger.Information($"Command sent having correlationId {command.CorrelationId} with data: {JObject.FromObject(command).ToString()}");
        }

        public static void LogEvent(this ILogger logger, AggregateEvent e)
        {
            logger.Information($"AggregateEvent writen for {e.EntityId} having correlationId {e.CorrelationId} and with data {JObject.FromObject(e).ToString()}");
        }

        public static void LogJson(this ILogger logger, string message, object e)
        {
            logger.Information($"{message} {JObject.FromObject(e).ToString()}");
        }
    }
}
