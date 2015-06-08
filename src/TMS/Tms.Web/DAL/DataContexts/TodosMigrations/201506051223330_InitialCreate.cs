namespace Tms.Web.DAL.DataContexts.TodosMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Todo.Tasks",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Description = c.String(),
                        Hours = c.Single(nullable: false),
                        Done = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Todo_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Todo.Todos", t => t.Todo_Id, cascadeDelete: true)
                .Index(t => t.Todo_Id);
            
            CreateTable(
                "Todo.Todos",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 80),
                        Description = c.String(),
                        State = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Todo.Tasks", "Todo_Id", "Todo.Todos");
            DropIndex("Todo.Tasks", new[] { "Todo_Id" });
            DropTable("Todo.Todos");
            DropTable("Todo.Tasks");
        }
    }
}
