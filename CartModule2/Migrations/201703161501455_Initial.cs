namespace CartModule2.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cart2",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    CartType = c.String(maxLength: 64),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cart", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.CartLineItem2",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    OuterId = c.String(maxLength: 64),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CartLineItem", t => t.Id)
                .Index(t => t.Id);
            //Convert  all exist ShoppingCart records to Cart2
            Sql("INSERT INTO dbo.Cart2 (Id) SELECT Id FROM dbo.Cart");
            //Convert  all exist LineItem records to LineItem2
            Sql("INSERT INTO dbo.CartLineItem2 (Id) SELECT Id FROM dbo.CartLineItem");
        }

        public override void Down()
        {
            DropForeignKey("dbo.CartLineItem2", "Id", "dbo.CartLineItem");
            DropForeignKey("dbo.Cart2", "Id", "dbo.Cart");

            DropIndex("dbo.CartLineItem2", new[] { "Id" });
            DropIndex("dbo.Cart2", new[] { "Id" });

            DropTable("dbo.CartLineItem2");
            DropTable("dbo.Cart2");

        }
    }
}
