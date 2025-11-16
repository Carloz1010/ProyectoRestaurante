using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoRestaurante.Migrations
{
    /// <inheritdoc />
    public partial class UsuariosAdministradores_Auth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuariosAdministradores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Contraseña = table.Column<string>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosAdministradores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdenItems_ProductoId",
                table: "OrdenItems",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenItems_Productos_ProductoId",
                table: "OrdenItems",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdenItems_Productos_ProductoId",
                table: "OrdenItems");

            migrationBuilder.DropTable(
                name: "UsuariosAdministradores");

            migrationBuilder.DropIndex(
                name: "IX_OrdenItems_ProductoId",
                table: "OrdenItems");
        }
    }
}
