using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class Estante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstanteId",
                table: "Insumos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Estantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estantes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_EstanteId",
                table: "Insumos",
                column: "EstanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insumos_Estantes_EstanteId",
                table: "Insumos",
                column: "EstanteId",
                principalTable: "Estantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insumos_Estantes_EstanteId",
                table: "Insumos");

            migrationBuilder.DropTable(
                name: "Estantes");

            migrationBuilder.DropIndex(
                name: "IX_Insumos_EstanteId",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "EstanteId",
                table: "Insumos");
        }
    }
}
