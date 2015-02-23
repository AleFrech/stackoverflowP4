namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Answermark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "Marked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "Marked");
        }
    }
}
