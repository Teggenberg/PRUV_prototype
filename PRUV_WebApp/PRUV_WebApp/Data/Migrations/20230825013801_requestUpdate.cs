using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRUV_WebApp.Data.Migrations
{
    public partial class requestUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Request",
                type: "int",
                nullable: false,
                defaultValue: 0);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Request");
        }
    }
}
