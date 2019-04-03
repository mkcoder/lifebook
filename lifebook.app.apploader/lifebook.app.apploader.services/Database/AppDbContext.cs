using System;
using lifebook.app.apploader.services.Models;
using lifebook.core.database.databaseprovider.service;
using Microsoft.EntityFrameworkCore;

namespace lifebook.app.apploader.services.Database
{
    public class AppDbContext : PostgreSQLDatabaseProvider
    {
        public AppDbContext() : base("App")
        {
        }
    }
}
