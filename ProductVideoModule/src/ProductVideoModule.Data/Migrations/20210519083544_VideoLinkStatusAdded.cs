using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductVideoModule.Data.Migrations
{
    public partial class VideoLinkStatusAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "VideoLinks",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "VideoLinks");
        }
    }
}
