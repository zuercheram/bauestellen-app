﻿// <auto-generated />
using System;
using Baustellen.App.Projects.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Baustellen.App.Projects.Api.Data.Migrations
{
    [DbContext(typeof(ProjectsDbContext))]
    [Migration("20250217133325_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Baustellen.App.Projects.Api.Models.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CreatedByOid")
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedAt")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ModifiedByOid")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.Property<string>("Telefon")
                        .HasColumnType("text");

                    b.Property<string>("Zip")
                        .HasColumnType("text");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

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

                    b.Property<string>("Lat")
                        .HasColumnType("text");

                    b.Property<string>("Lon")
                        .HasColumnType("text");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uuid");

                    b.Property<string>("ManagerName")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedAt")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ModifiedByOid")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<string>("ObjectCity")
                        .HasColumnType("text");

                    b.Property<string>("ObjectNumber")
                        .HasColumnType("text");

                    b.Property<string>("ObjectStreet")
                        .HasColumnType("text");

                    b.Property<string>("ObjectZip")
                        .HasColumnType("text");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("CustomerProject", b =>
                {
                    b.Property<Guid>("CustomersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProjectsId")
                        .HasColumnType("uuid");

                    b.HasKey("CustomersId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("CustomerProject");
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

            modelBuilder.Entity("CustomerProject", b =>
                {
                    b.HasOne("Baustellen.App.Projects.Api.Models.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Baustellen.App.Projects.Api.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Baustellen.App.Projects.Api.Models.Project", b =>
                {
                    b.Navigation("ExternalLinks");
                });
#pragma warning restore 612, 618
        }
    }
}
