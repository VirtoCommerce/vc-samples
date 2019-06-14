namespace External.PricingModule.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddPricelistEx : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PricelistEx",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    NewDescription = c.String(),
                    SecretCode = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pricelist", t => t.Id)
                .Index(t => t.Id);

            Sql("INSERT INTO dbo.PricelistEx (Id) SELECT Id FROM dbo.Pricelist");
        }

        public override void Down()
        {
            DropForeignKey("dbo.PricelistEx", "Id", "dbo.Pricelist");
            DropIndex("dbo.PricelistEx", new[] { "Id" });
            DropTable("dbo.PricelistEx");
        }
    }
}
