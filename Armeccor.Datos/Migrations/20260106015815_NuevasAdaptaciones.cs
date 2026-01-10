using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class NuevasAdaptaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Proveedores_IdProveedor",
                table: "Pedidos");

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "PedidoDetalleInsumos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "PedidoDetalleInsumos",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Proveedores_IdProveedor",
                table: "Pedidos",
                column: "IdProveedor",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Proveedores_IdProveedor",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "PedidoDetalleInsumos");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "PedidoDetalleInsumos");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Proveedores_IdProveedor",
                table: "Pedidos",
                column: "IdProveedor",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
