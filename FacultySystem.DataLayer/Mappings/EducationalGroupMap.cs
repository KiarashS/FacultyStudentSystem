using System.Data.Entity.ModelConfiguration;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.DataLayer.Mappings
{
    public class EducationalGroupMap : EntityTypeConfiguration<EducationalGroup>
    {
        public EducationalGroupMap()
        {
            Property(u => u.Name).
                IsRequired().
                HasMaxLength(150);
        }
    }
}
