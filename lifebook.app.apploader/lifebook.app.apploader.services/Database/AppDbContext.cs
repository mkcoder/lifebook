using System;
using System.Collections.Generic;
using lifebook.app.apploader.services.Models;
using lifebook.core.database.databaseprovider.service;
using Microsoft.EntityFrameworkCore;

namespace lifebook.app.apploader.services.Database
{
    public class AppDbContext : PostgreSQLDatabaseProvider
    {
        public DbSet<UserApps> UserApps { get; set; }
        public DbSet<App> Apps { get; set; }

        public AppDbContext() : base("Apps")
        {
            Host = "localhost";
            Port = "5432";
        }
    }
}
