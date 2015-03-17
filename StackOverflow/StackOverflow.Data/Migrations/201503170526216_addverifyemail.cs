namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addverifyemail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "VerifyEmail", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "VerifyEmail");
        }
    }
}
