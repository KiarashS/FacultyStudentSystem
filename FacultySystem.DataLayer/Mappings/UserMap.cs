using System.Data.Entity.ModelConfiguration;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.DataLayer.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            HasKey(u => u.Id);

            Property(u => u.Email).
                IsRequired();

            Property(u => u.Email).
                HasColumnType("VARCHAR").
                HasMaxLength(450);

            Property(u => u.FirstName).
                IsRequired().
                HasMaxLength(100);

            Property(u => u.LastName).
                IsRequired().
                HasMaxLength(100);

            Property(u => u.Password).
                IsRequired();

            Property(u => u.RegisterDate).
                IsRequired();
          
            Property(u => u.LastLoginDate).
                IsOptional();

            HasMany(r => r.Roles).
                WithMany(u => u.Users).
                Map(ur =>
                ur.ToTable("UserRolesJunction").
                MapLeftKey("UserId").
                MapRightKey("RoleId"));
        }
    }
}
