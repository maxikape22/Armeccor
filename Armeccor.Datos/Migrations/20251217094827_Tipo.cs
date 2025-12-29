using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class Tipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Factor",
                table: "UnidadConversiones",
                newName: "FactorConversion");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "UnidadMedidas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "UnidadMedidas");

            migrationBuilder.RenameColumn(
                name: "FactorConversion",
                table: "UnidadConversiones",
                newName: "Factor");
        }
    }
}
