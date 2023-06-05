namespace ContentManagementSystem.DataLayer.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedScopusCitation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Professors", "ScopusCitations");
            DropColumn("dbo.Professors", "ScopusTotalDocumentsCited");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Professors", "ScopusTotalDocumentsCited", c => c.Int());
            AddColumn("dbo.Professors", "ScopusCitations", c => c.Int());
        }
    }
}
