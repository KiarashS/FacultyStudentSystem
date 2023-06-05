using System.Data.Entity.ModelConfiguration;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.DataLayer.Mappings
{
    public class AcademicRankMap : EntityTypeConfiguration<AcademicRank>
    {
        public AcademicRankMap()
        {
            Property(u => u.Name).
                IsRequired().
                HasMaxLength(150);
        }
    }
}
