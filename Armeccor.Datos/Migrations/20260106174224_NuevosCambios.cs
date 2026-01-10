using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class NuevosCambios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_MedioDePagos_MedioDePagoId",
                table: "Entregas");

            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_Ordenes_OrdenId",
                table: "Entregas");

            migrationBuilder.DropForeignKey(
                name: "FK_InsumoDetalleOrdenes_Insumos_InsumoId",
                table: "InsumoDetalleOrdenes");

            migrationBuilder.DropForeignKey(
                name: "FK_InsumoDetalleOrdenes_Ordenes_OrdenId",
                table: "InsumoDetalleOrdenes");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordenes_Clientes_ClienteId",
                table: "Ordenes");

            migrationBuilder.DropForeignKey(
                name: "FK_Planos_Ordenes_OrdenId",
                table: "Planos");

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "Planos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Planos",
                type: "datetime2(7)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaBaja",
                table: "PedidoDetalleInsumos",
                type: "datetime2(7)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)");

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "Ordenes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Ordenes",
                type: "datetime2(7)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaBorrado",
                table: "Insumos",
                type: "datetime2(7)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "Insumos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "InsumoDetalleOrdenes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "InsumoDetalleOrdenes",
                type: "datetime2(7)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "Entregas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Entregas",
                type: "datetime2(7)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Clientes",
                type: "datetime2(7)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "Areas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Areas",
                type: "datetime2(7)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstaActivo",
                table: "AreaDetalleOrdenes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "AreaDetalleOrdenes",
                type: "datetime2(7)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EstaActivo", "FechaBaja" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EstaActivo", "FechaBaja" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EstaActivo", "FechaBaja" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EstaActivo", "FechaBaja" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EstaActivo", "FechaBaja" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EstaActivo", "FechaBaja" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EstaActivo", "FechaBaja" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EstaActivo", "FechaBaja" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "EstaActivo", "FechaBaja" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "EstaActivo", "FechaBaja" },
                values: new object[] { false, null });

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_MedioDePagos_MedioDePagoId",
                table: "Entregas",
                column: "MedioDePagoId",
                principalTable: "MedioDePagos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_Ordenes_OrdenId",
                table: "Entregas",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_InsumoDetalleOrdenes_Insumos_InsumoId",
                table: "InsumoDetalleOrdenes",
                column: "InsumoId",
                principalTable: "Insumos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_InsumoDetalleOrdenes_Ordenes_OrdenId",
                table: "InsumoDetalleOrdenes",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordenes_Clientes_ClienteId",
                table: "Ordenes",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Planos_Ordenes_OrdenId",
                table: "Planos",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_MedioDePagos_MedioDePagoId",
                table: "Entregas");

            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_Ordenes_OrdenId",
                table: "Entregas");

            migrationBuilder.DropForeignKey(
                name: "FK_InsumoDetalleOrdenes_Insumos_InsumoId",
                table: "InsumoDetalleOrdenes");

            migrationBuilder.DropForeignKey(
                name: "FK_InsumoDetalleOrdenes_Ordenes_OrdenId",
                table: "InsumoDetalleOrdenes");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordenes_Clientes_ClienteId",
                table: "Ordenes");

            migrationBuilder.DropForeignKey(
                name: "FK_Planos_Ordenes_OrdenId",
                table: "Planos");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "Planos");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Planos");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "Ordenes");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Ordenes");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "InsumoDetalleOrdenes");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "InsumoDetalleOrdenes");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "EstaActivo",
                table: "AreaDetalleOrdenes");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "AreaDetalleOrdenes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaBaja",
                table: "PedidoDetalleInsumos",
                type: "datetime2(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaBorrado",
                table: "Insumos",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_MedioDePagos_MedioDePagoId",
                table: "Entregas",
                column: "MedioDePagoId",
                principalTable: "MedioDePagos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_Ordenes_OrdenId",
                table: "Entregas",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InsumoDetalleOrdenes_Insumos_InsumoId",
                table: "InsumoDetalleOrdenes",
                column: "InsumoId",
                principalTable: "Insumos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InsumoDetalleOrdenes_Ordenes_OrdenId",
                table: "InsumoDetalleOrdenes",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordenes_Clientes_ClienteId",
                table: "Ordenes",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Planos_Ordenes_OrdenId",
                table: "Planos",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
