using System.Data.Entity.ModelConfiguration;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.DataLayer.Mappings
{
    public class FreeFieldMap : EntityTypeConfiguration<FreeField>
    {
        public FreeFieldMap()
        {
            Property(u => u.Name).
                IsRequired().
                HasMaxLength(300);

            Property(u => u.Value).
                //IsRequired().
                HasMaxLength(300);
        }
    }
}
