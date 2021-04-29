using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.AModule.Data.Migrations
{
    public partial class AddNewNumeric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewNumeric",
                table: "Seamless",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewNumeric",
                table: "Seamless");
        }
    }
}
