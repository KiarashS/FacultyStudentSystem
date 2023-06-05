using ContentManagementSystem.DomainClasses;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ContentManagementSystem.DataLayer.Context
{
    public class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ContentManagementSystem.DataLayer.Context.ApplicationDbContext context)
        {
            context.CreateUniqueIndex<User>(u => u.Email);
            context.CreateUniqueIndex<Professor>(p => p.PageId);
            //context.CreateUniqueIndex<SectionOrder>(so => new { so.ProfessorId, so.SectionName });
            //context.CreateUniqueIndex<EducationalDegree>(ed => ed.Name);
            //context.CreateUniqueIndex<AcademicRank>(ar => ar.Name);
            //context.CreateUniqueIndex<College>(c => c.Name);
            //context.CreateUniqueIndex<EducationalGroup>(eg => eg.Name);

            //context.CreateUniqueIndex<Professor>(p => new { p.UserId, p.PageId });

            //context.Database.ExecuteSqlCommand("ALTER TABLE ...");

            // Default Password: admin654321

            context.Set<Role>().AddOrUpdate(
              r => r.Name,
              new Role [] {
                  new Role
                  {
                      Name = "professor"
                  },
                  new Role
                  {
                      Name = "admin"
                  }
                }
            );

            context.SaveChanges();

            var adminRole = context.Set<Role>().Single(r => r.Name == "admin");
            var user = new User
            {
                Email = "kiarash.s@hotmail.com",
                FirstName = "مدیر کل",
                LastName = "سیستم جامع اطلاعات اساتید",
                RegisterDate = DateTime.UtcNow,
                Password = "1000:uv1j7WnNvqqidH+rDejBdfEAeboc3Nfu:JaU6BTCz9l6K3Xtg0x/ckGUB3dl4Cif+"
            };

            user.Roles.Add(adminRole);

            context.Set<User>().AddOrUpdate(
              u => u.Email,
              user
            );

            context.Set<EducationalDegree>().AddOrUpdate(
                ed => ed.Name ,
                new EducationalDegree
                {
                    Name = "--"
                });

            context.Set<AcademicRank>().AddOrUpdate(
                ed => ed.Name,
                new AcademicRank
                {
                    Name = "--"
                });

            context.Set<College>().AddOrUpdate(
                ed => ed.Name,
                new College
                {
                    Name = "--"
                });

            context.Set<EducationalGroup>().AddOrUpdate(
                ed => ed.Name,
                new EducationalGroup
                {
                    Name = "--"
                });
        }
    }
}
