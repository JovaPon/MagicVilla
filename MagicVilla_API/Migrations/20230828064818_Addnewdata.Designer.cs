﻿// <auto-generated />
using System;
using MagicVilla_API.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MagicVillaAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230828064818_Addnewdata")]
    partial class Addnewdata
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVilla_API.Modelos.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenidad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagenUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MetrosCuadrados")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ocupantes")
                        .HasColumnType("int");

                    b.Property<double>("Tarifa")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenidad = "",
                            Detalle = "detalle del producto",
                            FechaActualizacion = new DateTime(2023, 8, 28, 2, 48, 18, 479, DateTimeKind.Local).AddTicks(7814),
                            FechaCreacion = new DateTime(2023, 8, 28, 2, 48, 18, 479, DateTimeKind.Local).AddTicks(7777),
                            ImagenUrl = "",
                            MetrosCuadrados = 5,
                            Nombre = "jose Manuel",
                            Ocupantes = 5,
                            Tarifa = 5.0
                        },
                        new
                        {
                            Id = 2,
                            Amenidad = "",
                            Detalle = "detalle dffffel producto",
                            FechaActualizacion = new DateTime(2023, 8, 28, 2, 48, 18, 479, DateTimeKind.Local).AddTicks(7818),
                            FechaCreacion = new DateTime(2023, 8, 28, 2, 48, 18, 479, DateTimeKind.Local).AddTicks(7817),
                            ImagenUrl = "",
                            MetrosCuadrados = 90,
                            Nombre = "Adrian Perez",
                            Ocupantes = 6,
                            Tarifa = 90.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}