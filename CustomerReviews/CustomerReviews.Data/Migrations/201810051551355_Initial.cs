namespace CustomerReviews.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerReview",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AuthorNickname = c.String(maxLength: 128),
                        Content = c.String(nullable: false, maxLength: 1024),
                        IsActive = c.Boolean(nullable: false),
                        ProductId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustomerReview");
        }
    }
}
