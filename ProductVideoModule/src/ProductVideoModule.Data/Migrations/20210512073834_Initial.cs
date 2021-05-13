using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ProductVideoModule.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoLink",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    Url = table.Column<string>(maxLength: 256, nullable: true),
                    ProductId = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoLink", x => x.Id);
                });
            migrationBuilder.Sql(@"
            INSERT INTO[dbo].[VideoLink]
                   ([Id]
                  ,[Url]
                  ,[ProductId]
                  ,[CreatedDate])
             VALUES
                   ('id1'
                   , 'url1'
                   , 'productId1'
                   , GETDATE());
            INSERT INTO[dbo].[VideoLink]
                   ([Id]
                  ,[Url]
                  ,[ProductId]
                  ,[CreatedDate])
             VALUES
                   ('id2'
                   , 'url2'
                   , 'productId2'
                   , GETDATE());
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
