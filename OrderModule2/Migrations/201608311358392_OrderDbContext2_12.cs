namespace OrderModule2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderDbContext2_12 : DbMigration
    {
        public override void Up()
        {
            //Nothing TODO because just updated parent  db context. Instead this migration all queries will be falling with this error
            //The model backing the 'OrderRepository2' context has changed since the database was created. Consider using Code First Migrations to update the database
        }

        public override void Down()
        {
      
        }
    }
}
