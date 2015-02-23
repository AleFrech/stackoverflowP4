namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hadmark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "HavedMark", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "HavedMark");
        }
    }
}
