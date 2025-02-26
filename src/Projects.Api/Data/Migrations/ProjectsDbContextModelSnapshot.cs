﻿// <auto-generated />
using System;
using Baustellen.App.Projects.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Baustellen.App.Projects.Api.Data.Migrations
{
    [DbContext(typeof(ProjectsDbContext))]
    partial class ProjectsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Baustellen.App.Projects.Api.Models.ExternalLinks", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ExternalLinks");
                });

            modelBuilder.Entity("Baustellen.App.Projects.Api.Models.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Commissioning")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CreatedByOid")
                        .HasColumnType("integer");

                    b.Property<string>("CustomerCity")
                        .HasColumnType("text");

                    b.Property<string>("CustomerEmail")
                        .HasColumnType("text");

                    b.Property<string>("CustomerFirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CustomerHouseNumber")
                        .HasColumnType("text");

                    b.Property<string>("CustomerLastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CustomerStreet")
                        .HasColumnType("text");

                    b.Property<string>("CustomerTelefon")
                        .HasColumnType("text");

                    b.Property<string>("CustomerZip")
                        .HasColumnType("text");

                    b.Property<string>("Lat")
                        .HasColumnType("text");

                    b.Property<string>("Lon")
                        .HasColumnType("text");

                    b.Property<string>("ManagerEmail")
                        .HasColumnType("text");

                    b.Property<string>("ManagerName")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedAt")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ModifiedByOid")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ObjectCity")
                        .HasColumnType("text");

                    b.Property<string>("ObjectNumber")
                        .HasColumnType("text");

                    b.Property<string>("ObjectStreet")
                        .HasColumnType("text");

                    b.Property<string>("ObjectZip")
                        .HasColumnType("text");

                    b.Property<string>("RefNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Baustellen.App.Projects.Api.Models.ExternalLinks", b =>
                {
                    b.HasOne("Baustellen.App.Projects.Api.Models.Project", "Project")
                        .WithMany("ExternalLinks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Baustellen.App.Projects.Api.Models.Project", b =>
                {
                    b.Navigation("ExternalLinks");
                });
#pragma warning restore 612, 618
        }
    }
}
