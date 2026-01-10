using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class Propiedades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaUltimaPausa",
                table: "AreaDetalleOrdenes",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TiempoPausadoAcumuladoSegundos",
                table: "AreaDetalleOrdenes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaUltimaPausa",
                table: "AreaDetalleOrdenes");

            migrationBuilder.DropColumn(
                name: "TiempoPausadoAcumuladoSegundos",
                table: "AreaDetalleOrdenes");
        }
    }
}
