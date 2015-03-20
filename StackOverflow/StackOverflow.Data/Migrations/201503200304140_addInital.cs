namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        ConfirmPassword = c.String(),
                        Reputation = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        VerifyEmail = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Votes = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModififcationnDate = c.DateTime(nullable: false),
                        QuestionId = c.Guid(nullable: false),
                        Marked = c.Boolean(nullable: false),
                        Description = c.String(),
                        Owner_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        FatherId = c.Guid(nullable: false),
                        Votes = c.Int(nullable: false),
                        Owner_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Votes = c.Int(nullable: false),
                        Answers = c.Int(nullable: false),
                        Views = c.Int(nullable: false),
                        Description = c.String(),
                        Title = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        ModififcationnDate = c.DateTime(nullable: false),
                        HavedMark = c.Boolean(nullable: false),
                        Owner_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Owner_Id", "dbo.Accounts");
            DropForeignKey("dbo.Comments", "Owner_Id", "dbo.Accounts");
            DropForeignKey("dbo.Answers", "Owner_Id", "dbo.Accounts");
            DropIndex("dbo.Questions", new[] { "Owner_Id" });
            DropIndex("dbo.Comments", new[] { "Owner_Id" });
            DropIndex("dbo.Answers", new[] { "Owner_Id" });
            DropTable("dbo.Questions");
            DropTable("dbo.Comments");
            DropTable("dbo.Answers");
            DropTable("dbo.Accounts");
        }
    }
}
