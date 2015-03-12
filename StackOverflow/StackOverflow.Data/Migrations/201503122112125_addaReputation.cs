namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addaReputation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Reputation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Reputation");
        }
    }
}
