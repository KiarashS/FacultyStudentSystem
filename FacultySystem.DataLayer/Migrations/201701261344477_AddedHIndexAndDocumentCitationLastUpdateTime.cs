namespace ContentManagementSystem.DataLayer.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHIndexAndDocumentCitationLastUpdateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Professors", "HIndexAndDocumentCitationLastUpdateTime", c => c.DateTime());
            AddColumn("dbo.Professors", "ExternalResearchLastUpdateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Professors", "ExternalResearchLastUpdateTime");
            DropColumn("dbo.Professors", "HIndexAndDocumentCitationLastUpdateTime");
        }
    }
}
