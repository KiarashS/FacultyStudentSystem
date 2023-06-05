using System.Data.Entity.ModelConfiguration;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.DataLayer.Mappings
{
    public class EducationalDegreeMap : EntityTypeConfiguration<EducationalDegree>
    {
        public EducationalDegreeMap()
        {
            Property(u => u.Name).
                IsRequired().
                HasMaxLength(150);
        }
    }
}
