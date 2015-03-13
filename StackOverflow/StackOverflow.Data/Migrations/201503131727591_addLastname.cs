namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLastname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "LastName");
        }
    }
}
