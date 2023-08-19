using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRUV_WebApp.Data.Migrations
{
    public partial class numberTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestYear = table.Column<int>(type: "int", nullable: false),
                    RequestBrand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestModel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.RequestID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Request");
        }
    }
}
