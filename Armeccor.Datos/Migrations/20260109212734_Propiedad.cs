using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class Propiedad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "AreaDetalleOrdenes",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio",
                table: "AreaDetalleOrdenes",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TiempoEstimadoMinutos",
                table: "AreaDetalleOrdenes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TiempoPausadoAcumuladoMinutos",
                table: "AreaDetalleOrdenes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "AreaDetalleOrdenes");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "AreaDetalleOrdenes");

            migrationBuilder.DropColumn(
                name: "TiempoEstimadoMinutos",
                table: "AreaDetalleOrdenes");

            migrationBuilder.DropColumn(
                name: "TiempoPausadoAcumuladoMinutos",
                table: "AreaDetalleOrdenes");
        }
    }
}
