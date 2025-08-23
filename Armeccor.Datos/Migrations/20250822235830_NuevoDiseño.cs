using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class NuevoDiseño : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordenes_Areas_AreaId",
                table: "Ordenes");

            migrationBuilder.DropTable(
                name: "InsumosOrden");

            migrationBuilder.DropIndex(
                name: "IX_Ordenes_AreaId",
                table: "Ordenes");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Ordenes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "Tiempo",
                table: "Areas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Planos",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaPactada",
                table: "Ordenes",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicio",
                table: "Ordenes",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaEntrega",
                table: "Ordenes",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Detalle",
                table: "Insumos",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "AreaDetalleOrdenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenId = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tiempo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaDetalleOrdenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaDetalleOrdenes_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaDetalleOrdenes_Ordenes_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Ordenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsumoDetalleOrdenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsumoId = table.Column<int>(type: "int", nullable: false),
                    OrdenId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumoDetalleOrdenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsumoDetalleOrdenes_Insumos_InsumoId",
                        column: x => x.InsumoId,
                        principalTable: "Insumos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsumoDetalleOrdenes_Ordenes_OrdenId",
                        column: x => x.OrdenId,
                        principalTable: "Ordenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaDetalleOrdenes_AreaId",
                table: "AreaDetalleOrdenes",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumoDetalleOrdenes_InsumoId",
                table: "InsumoDetalleOrdenes",
                column: "InsumoId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumoDetalleOrdenes_OrdenId",
                table: "InsumoDetalleOrdenes",
                column: "OrdenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaDetalleOrdenes");

            migrationBuilder.DropTable(
                name: "InsumoDetalleOrdenes");

            migrationBuilder.DropColumn(
                name: "Detalle",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "Insumos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Planos",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaPactada",
                table: "Ordenes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicio",
                table: "Ordenes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaEntrega",
                table: "Ordenes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Ordenes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Tiempo",
                table: "Areas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InsumosOrden",
                columns: table => new
                {
                    OrdenId = table.Column<int>(type: "int", nullable: false),
                    InsumoId = table.Column<int>(type: "int", nullable: false),
                    CantidadUsada = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumosOrden", x => new { x.OrdenId, x.InsumoId });
                    table.ForeignKey(
                        name: "FK_InsumosOrden_Insumos_InsumoId",
                        column: x => x.InsumoId,
                        principalTable: "Insumos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsumosOrden_Ordenes_OrdenId",
                        column: x => x.OrdenId,
                        principalTable: "Ordenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_AreaId",
                table: "Ordenes",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosOrden_InsumoId",
                table: "InsumosOrden",
                column: "InsumoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordenes_Areas_AreaId",
                table: "Ordenes",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
