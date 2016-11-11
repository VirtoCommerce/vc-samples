namespace MemberExtensionSampleModule.Web.Migrations
{
    using System;
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
                .ForeignKey("dbo.Contact", t => t.Id)
                .Index(t => t.Id);

            //Convert  all exist Contact records to Contact2
            Sql("INSERT INTO dbo.Contact2 (Id) SELECT Id FROM dbo.Contact");

        }
        
        public override void Down()
        {
            DropIndex("dbo.Contact2", new[] { "Id" });
          
        }
    }
}
