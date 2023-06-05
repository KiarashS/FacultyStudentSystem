namespace ContentManagementSystem.DataLayer.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedPageIdType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Professors", "PageId", c => c.String(nullable: false, maxLength: 450));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Professors", "PageId", c => c.String(nullable: false, maxLength: 450, unicode: false));
        }
    }
}
