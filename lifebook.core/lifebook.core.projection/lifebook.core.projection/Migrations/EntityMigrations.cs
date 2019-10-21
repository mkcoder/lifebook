using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using lifebook.core.projection.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

namespace lifebook.core.projection.Migrations
{
    public class EntityMigrations : Migration
    {
        protected override void Up([NotNullAttribute] MigrationBuilder migrationBuilder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => typeof(EntityProjection).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();
            foreach (var type in assemblies)
            {
                migrationBuilder.CreateTable(
                    type.Name,
                    table => new
                    {
                        Key = table.Column<Guid>(),
                        JSON = table.Column<string>(type: "jsonb"),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("Key", x => x.Key);
                    }
                );
            }            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "__EFMigrationsHistory");
            base.Down(migrationBuilder);
        }
    }
}
