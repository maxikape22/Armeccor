using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armeccor.Datos.Migrations
{
    /// <inheritdoc />
    public partial class TablaNueva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventosNotificados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenciaId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosNotificados", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventosNotificados");
        }
    }
}
