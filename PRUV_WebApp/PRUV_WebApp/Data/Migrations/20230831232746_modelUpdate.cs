using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRUV_WebApp.Data.Migrations
{
    public partial class modelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewBrand",
                table: "Request",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewBrand",
                table: "Request");
        }
    }
}
