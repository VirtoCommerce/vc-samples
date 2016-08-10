namespace MemberExtensionSampleModule.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SupplierReviews : DbMigration
    {
        public override void Up()
        {
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
            DropIndex("dbo.SupplierReview", new[] { "SupplierId" });
            DropTable("dbo.SupplierReview");
        }
    }
}
