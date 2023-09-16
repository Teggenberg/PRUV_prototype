using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRUV_WebApp.Data.Migrations
{
    public partial class RequestImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "RequestImage",
            columns: table => new
            {
                ImageId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RequestId = table.Column<int>(type: "int", nullable: true),
                RequestImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)

            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ImageId", x => x.ImageId);
            });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestImage");

        }
    }
}
