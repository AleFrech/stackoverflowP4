namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addimgUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "ImageUrl");
        }
    }
}
