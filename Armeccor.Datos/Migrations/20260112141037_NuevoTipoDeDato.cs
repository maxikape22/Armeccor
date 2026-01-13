using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class NuevoTipoDeDato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Tiempo",
                table: "AreaDetalleOrdenes",
                type: "date",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Tiempo",
                table: "AreaDetalleOrdenes",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
