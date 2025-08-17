using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class RestriccionesActualizadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordenes_Areas_AreaId",
                table: "Ordenes");

            migrationBuilder.AlterColumn<int>(
                name: "AreaId",
                table: "Ordenes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordenes_Areas_AreaId",
                table: "Ordenes",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordenes_Areas_AreaId",
                table: "Ordenes");

            migrationBuilder.AlterColumn<int>(
                name: "AreaId",
                table: "Ordenes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordenes_Areas_AreaId",
                table: "Ordenes",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
