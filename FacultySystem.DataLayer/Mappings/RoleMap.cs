using System.Data.Entity.ModelConfiguration;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.DataLayer.Mappings
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            HasKey(u => u.Id);

            Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
