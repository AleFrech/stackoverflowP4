namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addquestion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Votes = c.Int(nullable: false),
                        Description = c.String(),
                        Title = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        ModififcationnDate = c.DateTime(nullable: false),
                        Owner_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
      
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Owner_Id", "dbo.Accounts");
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.Questions", new[] { "Owner_Id" });
            DropTable("dbo.Answers");
            DropTable("dbo.Questions");
        }
    }
}
