using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.AModule.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seamless",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    Numeric = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Discriminator = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seamless", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seamless");
        }
    }
}
