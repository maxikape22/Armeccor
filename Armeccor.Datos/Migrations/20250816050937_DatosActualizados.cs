using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class DatosActualizados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_Ordenes_OrdenId",
                table: "Entregas");

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_Ordenes_OrdenId",
                table: "Entregas",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_Ordenes_OrdenId",
                table: "Entregas");

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_Ordenes_OrdenId",
                table: "Entregas",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
