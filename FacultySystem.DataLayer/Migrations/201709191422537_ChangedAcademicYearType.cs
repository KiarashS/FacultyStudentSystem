namespace ContentManagementSystem.DataLayer.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedAcademicYearType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Lessons", "AcademicYear", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Lessons", "AcademicYear", c => c.Int());
        }
    }
}
