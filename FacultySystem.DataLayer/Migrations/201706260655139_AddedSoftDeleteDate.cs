namespace ContentManagementSystem.DataLayer.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSoftDeleteDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Professors", "SoftDeleteDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Professors", "SoftDeleteDate");
        }
    }
}
