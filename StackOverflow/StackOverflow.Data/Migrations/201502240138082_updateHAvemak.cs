namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateHAvemak : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Questions", "HavedMark");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "HavedMark", c => c.Boolean(nullable: false));
        }
    }
}
