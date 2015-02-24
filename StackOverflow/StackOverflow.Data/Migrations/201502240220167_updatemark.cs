namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "Marked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Questions", "HavedMark", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "HavedMark");
            DropColumn("dbo.Answers", "Marked");
        }
    }
}
