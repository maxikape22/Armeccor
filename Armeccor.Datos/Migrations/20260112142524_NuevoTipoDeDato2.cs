using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class NuevoTipoDeDato2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Tiempo",
                table: "AreaDetalleOrdenes",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Tiempo",
                table: "AreaDetalleOrdenes",
                type: "date",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }
    }
}
