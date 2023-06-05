using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;

namespace ContentManagementSystem.DataLayer
{
    public static class CorrectYeKe
    {
        public static void ApplyCorrectYeKe(this ObjectContext ctx)
        {
            if (ctx == null)
                return;

            var changedEntities = ctx.ObjectStateManager.GetObjectStateEntries(
                EntityState.Added | EntityState.Modified
                );

            foreach (var entity in changedEntities)
            {
                if (entity.Entity == null) continue;

                var propertyInfos = entity.Entity.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                    ).Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                var pr = new PropertyReflector();

                foreach (var propertyInfo in propertyInfos)
                {
                    var propName = propertyInfo.Name;
                    var val = pr.GetValue(entity.Entity, propName);
                    if (val != null)
                    {
                        pr.SetValue(
                            entity.Entity,
                            propName,
                            val.ToString().ApplyUnifiedYeKe());
                    }
                }
            }
        }
    }
}
