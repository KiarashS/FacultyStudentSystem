using System.Data.Entity.ModelConfiguration;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.DataLayer.Mappings
{
    public class WeeklyProgramMap : EntityTypeConfiguration<WeeklyProgram>
    {
        public WeeklyProgramMap()
        {
            Property(u => u.StartTime).
                IsRequired();

            Property(u => u.EndTime).
                IsRequired();
        }
    }
}
