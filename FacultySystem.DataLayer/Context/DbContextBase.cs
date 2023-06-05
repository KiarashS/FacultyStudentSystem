using StackExchange.Exceptional;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System;
using EFSecondLevelCache;

namespace ContentManagementSystem.DataLayer.Context
{
    public abstract class DbContextBase : DbContext, IUnitOfWork
    {

        #region IUnitOfWork Members
        public void RejectChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;

                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        //public int SaveAllChanges()
        //{
        //    return SaveAllChanges(isAjaxCaller: true, enableDetectChanges: true, validateOnSaveEnabled: true);
        //}
        public int SaveAllChanges(bool isAjaxCaller = true, bool enableDetectChanges = true, bool validateOnSaveEnabled = true, bool invalidateCacheDependencies = true)
        {
            var changedEntityNames = getChangedEntityNames();
            Configuration.AutoDetectChangesEnabled = enableDetectChanges;

            if (!validateOnSaveEnabled)
                Configuration.ValidateOnSaveEnabled = false;

            //var result = 0;
            if (isAjaxCaller)
            {
                try
                {
                    //this.InvalidateSecondLevelCache();
                    //((IObjectContextAdapter)this).ObjectContext.ApplyCorrectYeKe();

                    var result = base.SaveChanges();
                    if (invalidateCacheDependencies)
                    {
                        new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
                    }
                    return result;
                }
                catch (DbEntityValidationException validationException)
                {
                    // ...
                    //ErrorSignal.FromCurrentContext().Raise(validationException);
                    ErrorStore.LogException(validationException, HttpContext.Current, true, true);
                    // throw;
                }
                catch (DbUpdateConcurrencyException concurrencyException)
                {
                    // ...
                    //ErrorSignal.FromCurrentContext().Raise(concurrencyException);
                    ErrorStore.LogException(concurrencyException, HttpContext.Current, true, true);
                    // throw;
                }
                catch (DbUpdateException updateException)
                {
                    // ...
                    //ErrorSignal.FromCurrentContext().Raise(updateException);
                    ErrorStore.LogException(updateException, HttpContext.Current, true, true);
                    // throw;
                }
                finally
                {
                    if (!enableDetectChanges)
                        Configuration.AutoDetectChangesEnabled = true;

                    if (!validateOnSaveEnabled)
                        Configuration.ValidateOnSaveEnabled = true;
                }

                // return result for error;
                return -1;
            }
            else
            {
                try
                {
                    //this.InvalidateSecondLevelCache();
                    //((IObjectContextAdapter)this).ObjectContext.ApplyCorrectYeKe();

                    var result = base.SaveChanges();
                    if (invalidateCacheDependencies)
                    {
                        new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
                    }
                    return result;
                }
                catch (DbEntityValidationException validationException)
                {
                    // ...
                    // ErrorSignal.FromCurrentContext().Raise(validationException);

                    throw;
                }
                catch (DbUpdateConcurrencyException concurrencyException)
                {
                    // ...
                    // ErrorSignal.FromCurrentContext().Raise(concurrencyException);
                    throw;
                }
                catch (DbUpdateException updateException)
                {
                    // ...
                    // ErrorSignal.FromCurrentContext().Raise(updateException);

                    throw;
                }
                finally
                {
                    if (!enableDetectChanges)
                        Configuration.AutoDetectChangesEnabled = true;

                    if (!validateOnSaveEnabled)
                        Configuration.ValidateOnSaveEnabled = true;
                }

                //return result for error;
                return -1;

            }
        }

        //IDbSet<TEntity> IUnitOfWork.Set<TEntity>() where TEntity: class
        //{
        //    return Set<TEntity>();
        //}
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        //Access directly from the DbContext to the underlying ObjectContext
        public IObjectContextAdapter Core
        {
            get
            {
                return (this);
            }
        }

        public DbContextConfiguration Config
        {
            get
            {
                return (Configuration);
            }
        }

        #endregion


        public IEnumerable<TEntity> AddThisRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            return ((DbSet<TEntity>)this.Set<TEntity>()).AddRange(entities);
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public IList<T> GetRows<T>(string sql, params object[] parameters) where T : class
        {
            return Database.SqlQuery<T>(sql, parameters).ToList();
        }

        public void ForceDatabaseInitialize()
        {
            this.Database.Initialize(force: true);
        }

        private string[] getChangedEntityNames()
        {
            return this.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added ||
                            x.State == EntityState.Modified ||
                            x.State == EntityState.Deleted)
                .Select(x => System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(x.Entity.GetType()).FullName)
                .Distinct()
                .ToArray();
        }
    }
}