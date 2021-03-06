﻿using System;

namespace lifebook.core.eventstore.domain.models
{
    public sealed class StreamCategorySpecifier
    {
        public string Service { get; }
        public string Instance { get; }
        public string Category { get; }
        public Guid? AggregateId { get; }

        public StreamCategorySpecifier(string service, string instance, string category) : this(service, instance, category, null)
        {
        }

        public StreamCategorySpecifier(string service, string instance, string category, Guid? aggregateId)
        {
            Service = service;
            Instance = instance;
            Category = category;
            AggregateId = aggregateId;
        }

        public string GetCategoryStream() => $"{Service}.{Instance}.{Category}";
        public string GetCategoryStreamWithAggregateId() => $"{GetCategoryStream()}-{GetAggregateId(AggregateId.Value)}";

        private string GetAggregateId(Guid guid) => guid.ToString().Replace("-", "");
    }
}