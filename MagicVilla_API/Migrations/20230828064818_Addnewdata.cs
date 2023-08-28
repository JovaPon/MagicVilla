using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class Addnewdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Detalle",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "detalle del producto", new DateTime(2023, 8, 28, 2, 48, 18, 479, DateTimeKind.Local).AddTicks(7814), new DateTime(2023, 8, 28, 2, 48, 18, 479, DateTimeKind.Local).AddTicks(7777), "", 5, "jose Manuel", 5, 5.0 },
                    { 2, "", "detalle dffffel producto", new DateTime(2023, 8, 28, 2, 48, 18, 479, DateTimeKind.Local).AddTicks(7818), new DateTime(2023, 8, 28, 2, 48, 18, 479, DateTimeKind.Local).AddTicks(7817), "", 90, "Adrian Perez", 6, 90.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "Detalle",
                table: "Villas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
