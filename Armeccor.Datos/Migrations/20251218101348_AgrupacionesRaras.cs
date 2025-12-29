using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class AgrupacionesRaras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LongitudPorUnidad",
                table: "UnidadMedidas",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnidadesPorAgrupacion",
                table: "UnidadMedidas",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongitudPorUnidad",
                table: "UnidadMedidas");

            migrationBuilder.DropColumn(
                name: "UnidadesPorAgrupacion",
                table: "UnidadMedidas");
        }
    }
}
