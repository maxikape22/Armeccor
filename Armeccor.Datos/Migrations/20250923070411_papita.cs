using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class papita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreArea = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DNI = table.Column<int>(type: "int", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Insumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnidadMedida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CantDisponible = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insumos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Planos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RutaSVG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutaOriginal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ordenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NroOT = table.Column<int>(type: "int", nullable: false),
                    NombreOrden = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "date", nullable: false),
                    FechaPactada = table.Column<DateTime>(type: "date", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "date", nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    PlanoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ordenes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ordenes_Planos_PlanoId",
                        column: x => x.PlanoId,
                        principalTable: "Planos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AreaDetalleOrdenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenId = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: true),
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
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AreaDetalleOrdenes_Ordenes_OrdenId",
                        column: x => x.OrdenId,
                        principalTable: "Ordenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entregas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedioDePago = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Entregado = table.Column<bool>(type: "bit", nullable: false),
                    OrdenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entregas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entregas_Ordenes_OrdenId",
                        column: x => x.OrdenId,
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
                    InsumoId = table.Column<int>(type: "int", nullable: true),
                    OrdenId = table.Column<int>(type: "int", nullable: true),
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

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "NombreArea" },
                values: new object[,]
                {
                    { 1, "Mecanizado" },
                    { 2, "Soldadura" },
                    { 3, "Pintura" },
                    { 4, "Calidad" },
                    { 5, "Embalaje" },
                    { 6, "Logística" },
                    { 7, "Montaje" },
                    { 8, "Mantenimiento" },
                    { 9, "Administración" },
                    { 10, "Compras" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaDetalleOrdenes_AreaId",
                table: "AreaDetalleOrdenes",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaDetalleOrdenes_OrdenId",
                table: "AreaDetalleOrdenes",
                column: "OrdenId");

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_OrdenId",
                table: "Entregas",
                column: "OrdenId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumoDetalleOrdenes_InsumoId",
                table: "InsumoDetalleOrdenes",
                column: "InsumoId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumoDetalleOrdenes_OrdenId",
                table: "InsumoDetalleOrdenes",
                column: "OrdenId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_ClienteId",
                table: "Ordenes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_PlanoId",
                table: "Ordenes",
                column: "PlanoId");

            migrationBuilder.CreateIndex(
                name: "NroOT_UQ",
                table: "Ordenes",
                column: "NroOT",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaDetalleOrdenes");

            migrationBuilder.DropTable(
                name: "Entregas");

            migrationBuilder.DropTable(
                name: "InsumoDetalleOrdenes");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Insumos");

            migrationBuilder.DropTable(
                name: "Ordenes");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Planos");
        }
    }
}
