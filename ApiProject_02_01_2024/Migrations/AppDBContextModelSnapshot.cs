﻿// <auto-generated />
using System;
using ApiProject_02_01_2024.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiProject_02_01_2024.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ApiProject_02_01_2024.Models.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BankCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("LDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LIP")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LMAC")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("ModifyDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("BankCode")
                        .IsUnique();

                    b.HasIndex("BankName")
                        .IsUnique();

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("ApiProject_02_01_2024.Models.Designation", b =>
                {
                    b.Property<int>("DesignationAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DesignationAutoId"));

                    b.Property<string>("DesignationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DesignationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("LDate")
                        .HasColumnType("datetime");

                    b.Property<string>("LIP")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LMAC")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("ModifyDate")
                        .HasColumnType("datetime");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("DesignationAutoId");

                    b.ToTable("Designations");
                });
#pragma warning restore 612, 618
        }
    }
}
