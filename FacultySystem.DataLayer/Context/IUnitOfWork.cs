using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ContentManagementSystem.DataLayer.Context
{
    public interface IUnitOfWork : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveAllChanges(bool isAjaxCaller = true, bool enableDetectChanges = true, bool validateOnSaveEnabled = true, bool invalidateCacheDependencies = true);
        void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class;
        void RejectChanges();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        IList<T> GetRows<T>(string sql, params object[] parameters) where T : class;
        IEnumerable<TEntity> AddThisRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void ForceDatabaseInitialize();
        IObjectContextAdapter Core { get; }
        Database Database { get; }
        DbContextConfiguration Config { get; }
    }
}