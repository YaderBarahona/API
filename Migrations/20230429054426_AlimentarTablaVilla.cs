using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AlimentarTablaVilla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImageURL", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[] { 1, "", "Detalle de la villa", new DateTime(2023, 4, 28, 23, 44, 26, 400, DateTimeKind.Local).AddTicks(5169), new DateTime(2023, 4, 28, 23, 44, 26, 400, DateTimeKind.Local).AddTicks(5156), "", 100, "Villa Real", 4, 150.0 });

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImageURL", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[] { 2, "", "Detalle de la villa 2", new DateTime(2023, 4, 28, 23, 44, 26, 400, DateTimeKind.Local).AddTicks(5171), new DateTime(2023, 4, 28, 23, 44, 26, 400, DateTimeKind.Local).AddTicks(5171), "", 1000, "Villa Real 2", 5, 1500.0 });
        }

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
        }
    }
}
