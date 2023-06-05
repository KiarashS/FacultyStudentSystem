using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ContentManagementSystem.DomainClasses;
using System.Reflection;
using System.Data.Entity.ModelConfiguration;
using System;

namespace ContentManagementSystem.DataLayer.Context
{
    public class ApplicationDbContext : DbContextBase
    {
        //public DbSet<Category> Categories { set; get; }
        //public DbSet<Product> Products { set; get; }
        //public DbSet<Address> Addresses { set; get; }

        /// <summary>
        /// It looks for a connection string named connectionString1 in the web.config file.
        /// </summary>
        //public ApplicationDbContext()
        //    : base("connectionString")
        //{
        //    //this.Database.Log = data => System.Diagnostics.Debug.WriteLine(data);
        //}

        /// <summary>
        /// To change the connection string at runtime. See the SmObjectFactory class for more info.
        /// </summary>
        //public ApplicationDbContext(string connectionString)
        //    : base(connectionString)
        //{
        //    //Note: defaultConnectionFactory in the web.config file should be set.
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var entitiesAsm = Assembly.GetAssembly(typeof(DomainClassBase));
            var currentAsm = Assembly.GetExecutingAssembly();

            LoadEntityConfigurations(currentAsm, modelBuilder, "ContentManagementSystem.DataLayer.Mappings");
            LoadEntities(entitiesAsm, modelBuilder, "ContentManagementSystem.DomainClasses");

            base.OnModelCreating(modelBuilder);

            //builder.Entity<ApplicationUser>().ToTable("Users");
            //builder.Entity<CustomRole>().ToTable("Roles");
            //builder.Entity<CustomUserClaim>().ToTable("UserClaims");
            //builder.Entity<CustomUserRole>().ToTable("UserRoles");
            //builder.Entity<CustomUserLogin>().ToTable("UserLogins");
        }
        void LoadEntityConfigurations(Assembly asm, DbModelBuilder modelBuilder, string nameSpace)
        {
            var configurations = asm.GetTypes()
                                    .Where(type => type.BaseType != null &&
                                           type.Namespace == nameSpace &&
                                           type.BaseType.IsGenericType &&
                                           (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>) ||
                                           type.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>)))
                                    .ToList();

            configurations.ForEach(type =>
            {
                dynamic instance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(instance);
            });
        }

        void LoadEntities(Assembly asm, DbModelBuilder modelBuilder, string nameSpace)
        {
            var entityTypes = asm.GetTypes()
                                    .Where(type => type.BaseType != null &&
                                           type.Namespace == nameSpace &&
                                           type.BaseType.IsAbstract &&
                                           type.BaseType == typeof(DomainClassBase))
                                    .ToList();

            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");
            entityTypes.ForEach(type =>
            {
                entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
            });
        }

    }
}