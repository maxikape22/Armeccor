using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class Tablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "Insumos");

            migrationBuilder.AlterColumn<int>(
                name: "IdProveedor",
                table: "Pedidos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "CantDisponible",
                table: "Insumos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBorrado",
                table: "Insumos",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnidadMedidaId",
                table: "Insumos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CantidadDescontada",
                table: "InsumoDetalleOrdenes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CantidadPendiente",
                table: "InsumoDetalleOrdenes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Insuficiente",
                table: "InsumoDetalleOrdenes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UnidadMedidas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EsBase = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedidas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadConversiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnidadOrigenId = table.Column<int>(type: "int", nullable: false),
                    UnidadDestinoId = table.Column<int>(type: "int", nullable: false),
                    Factor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadConversiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnidadConversiones_UnidadMedidas_UnidadDestinoId",
                        column: x => x.UnidadDestinoId,
                        principalTable: "UnidadMedidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnidadConversiones_UnidadMedidas_UnidadOrigenId",
                        column: x => x.UnidadOrigenId,
                        principalTable: "UnidadMedidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_UnidadMedidaId",
                table: "Insumos",
                column: "UnidadMedidaId");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadConversiones_UnidadDestinoId",
                table: "UnidadConversiones",
                column: "UnidadDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadConversiones_UnidadOrigenId",
                table: "UnidadConversiones",
                column: "UnidadOrigenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insumos_UnidadMedidas_UnidadMedidaId",
                table: "Insumos",
                column: "UnidadMedidaId",
                principalTable: "UnidadMedidas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insumos_UnidadMedidas_UnidadMedidaId",
                table: "Insumos");

            migrationBuilder.DropTable(
                name: "UnidadConversiones");

            migrationBuilder.DropTable(
                name: "UnidadMedidas");

            migrationBuilder.DropIndex(
                name: "IX_Insumos_UnidadMedidaId",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "FechaBorrado",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "UnidadMedidaId",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "CantidadDescontada",
                table: "InsumoDetalleOrdenes");

            migrationBuilder.DropColumn(
                name: "CantidadPendiente",
                table: "InsumoDetalleOrdenes");

            migrationBuilder.DropColumn(
                name: "Insuficiente",
                table: "InsumoDetalleOrdenes");

            migrationBuilder.AlterColumn<int>(
                name: "IdProveedor",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CantDisponible",
                table: "Insumos",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Insumos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadMedida",
                table: "Insumos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
