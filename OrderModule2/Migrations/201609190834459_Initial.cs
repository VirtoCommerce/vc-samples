namespace OrderModule2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
            "dbo.CustomerOrder2",
            c => new
            {
                Id = c.String(nullable: false, maxLength: 128),
                NewField = c.String(),
            })
            .PrimaryKey(t => t.Id)
            .ForeignKey("dbo.CustomerOrder", t => t.Id)
            .Index(t => t.Id);

            CreateTable(
                "dbo.OrderInvoice",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    CustomerId = c.String(),
                    CustomerName = c.String(),
                    CustomerOrder2Id = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderOperation", t => t.Id)
                .ForeignKey("dbo.CustomerOrder2", t => t.CustomerOrder2Id)
                .Index(t => t.Id)
                .Index(t => t.CustomerOrder2Id);
            //Convert  all exist CustomerOrder records to CustomerOrder2
            Sql("INSERT INTO dbo.CustomerOrder2 (Id) SELECT Id FROM dbo.CustomerOrder");

        }

        public override void Down()
        {

            DropForeignKey("dbo.OrderInvoice", "CustomerOrder2Id", "dbo.CustomerOrder2");
            DropForeignKey("dbo.OrderInvoice", "Id", "dbo.OrderOperation");
            DropForeignKey("dbo.CustomerOrder2", "Id", "dbo.CustomerOrder");
            DropTable("dbo.OrderInvoice");
            DropTable("dbo.CustomerOrder2");

        }
    }
}
