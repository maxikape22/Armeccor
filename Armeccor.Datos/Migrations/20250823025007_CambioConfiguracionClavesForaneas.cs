using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class CambioConfiguracionClavesForaneas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AreaDetalleOrdenes_Ordenes_AreaId",
                table: "AreaDetalleOrdenes");

            migrationBuilder.CreateIndex(
                name: "IX_AreaDetalleOrdenes_OrdenId",
                table: "AreaDetalleOrdenes",
                column: "OrdenId");

            migrationBuilder.AddForeignKey(
                name: "FK_AreaDetalleOrdenes_Ordenes_OrdenId",
                table: "AreaDetalleOrdenes",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AreaDetalleOrdenes_Ordenes_OrdenId",
                table: "AreaDetalleOrdenes");

            migrationBuilder.DropIndex(
                name: "IX_AreaDetalleOrdenes_OrdenId",
                table: "AreaDetalleOrdenes");

            migrationBuilder.AddForeignKey(
                name: "FK_AreaDetalleOrdenes_Ordenes_AreaId",
                table: "AreaDetalleOrdenes",
                column: "AreaId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
