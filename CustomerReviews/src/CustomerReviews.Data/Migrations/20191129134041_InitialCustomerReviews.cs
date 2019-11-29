using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerReviews.Data.Migrations
{
    public partial class InitialCustomerReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerReview",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 64, nullable: true),
                    AuthorNickname = table.Column<string>(maxLength: 128, nullable: true),
                    Content = table.Column<string>(maxLength: 1024, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ProductId = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerReview", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerReview");
        }
    }
}
