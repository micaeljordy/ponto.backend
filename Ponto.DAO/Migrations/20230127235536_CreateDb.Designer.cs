﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ponto.DAL.Objects;

#nullable disable

namespace Ponto.DAL.Migrations
{
    [DbContext(typeof(PontoContext))]
    [Migration("20230127235536_CreateDb")]
    partial class CreateDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Ponto.DAL.Objects.Entry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("PunchDateTime")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("PunchType")
                        .HasColumnType("int");

                    b.Property<long>("RegisterId")
                        .HasColumnType("bigint");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "RegisterId" }, "RegisterId");

                    b.ToTable("entry", (string)null);
                });

            modelBuilder.Entity("Ponto.DAL.Objects.Register", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("EmployeeName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PunchDate")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("register", (string)null);
                });

            modelBuilder.Entity("Ponto.DAL.Objects.Entry", b =>
                {
                    b.HasOne("Ponto.DAL.Objects.Register", "Register")
                        .WithMany("Entries")
                        .HasForeignKey("RegisterId")
                        .IsRequired()
                        .HasConstraintName("entry_ibfk_1");

                    b.Navigation("Register");
                });

            modelBuilder.Entity("Ponto.DAL.Objects.Register", b =>
                {
                    b.Navigation("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
