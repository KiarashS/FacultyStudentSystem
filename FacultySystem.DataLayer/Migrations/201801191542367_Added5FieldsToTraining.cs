namespace ContentManagementSystem.DataLayer.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added5FieldsToTraining : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingRecords", "FromTime", c => c.Int());
            AddColumn("dbo.TrainingRecords", "ToTime", c => c.Int());
            AddColumn("dbo.TrainingRecords", "Teacher", c => c.String());
            AddColumn("dbo.TrainingRecords", "Participant", c => c.String());
            AddColumn("dbo.TrainingRecords", "Secretary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrainingRecords", "Secretary");
            DropColumn("dbo.TrainingRecords", "Participant");
            DropColumn("dbo.TrainingRecords", "Teacher");
            DropColumn("dbo.TrainingRecords", "ToTime");
            DropColumn("dbo.TrainingRecords", "FromTime");
        }
    }
}
