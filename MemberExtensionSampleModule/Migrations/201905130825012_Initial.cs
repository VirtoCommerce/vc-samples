namespace MemberExtensionSampleModule.Web.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contact2",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    JobTitle = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contact", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);

            // Convert all exist Contact records to Contact2
            Sql("INSERT INTO dbo.Contact2 (Id) SELECT Id FROM dbo.Contact");

            CreateTable(
                "dbo.Supplier",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    ContractNumber = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.SupplierReview",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Review = c.String(),
                        SupplierId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Supplier", t => t.SupplierId)
                .Index(t => t.SupplierId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Supplier", "Id", "dbo.Member");
            DropForeignKey("dbo.Contact2", "Id", "dbo.Contact");
            DropForeignKey("dbo.SupplierReview", "SupplierId", "dbo.Supplier");

            DropIndex("dbo.Supplier", new[] { "Id" });
            DropIndex("dbo.Contact2", new[] { "Id" });
            DropIndex("dbo.SupplierReview", new[] { "SupplierId" });
            
            DropTable("dbo.Supplier");
            DropTable("dbo.Contact2");
            DropTable("dbo.SupplierReview");
        }
    }
}
