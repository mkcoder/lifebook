﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using lifebook.app.apploader.services.Database;

namespace lifebook.app.apploader.services.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190412032605_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("lifebook.app.apploader.services.Models.App", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Icon");

                    b.Property<string>("Name");

                    b.Property<string>("ServiceName");

                    b.HasKey("Id");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("lifebook.app.apploader.services.Models.UserApps", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AppId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserApps");
                });
#pragma warning restore 612, 618
        }
    }
}
