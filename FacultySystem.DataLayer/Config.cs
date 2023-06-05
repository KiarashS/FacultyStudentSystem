//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Data.Entity;
//using System.Data.Entity.Core.Common;
//using System.Data.Entity.Core.EntityClient;
//using System.Data.Entity.Infrastructure;
//using System.Data.Entity.Infrastructure.DependencyResolution;
//using System.Data.Entity.Migrations.Sql;
//using System.Data.Entity.SqlServer;
//using System.Reflection;
//using EFCache;
////using HibernatingRhinos.Profiler.Appender.EntityFramework;
////using HibernatingRhinos.Profiler.Appender.ProfiledDataAccess;

//namespace ContentManagementSystem.DataLayer
//{
//    public class ProfiledDbConfiguration : DbConfiguration
//	{
//		public ProfiledDbConfiguration()
//		{
//            var transactionHandler = new CacheTransactionHandler(new InMemoryCache());

//            AddInterceptor(transactionHandler);

//            var cachingPolicy = new CachingPolicy();

//            Loaded +=
//              (sender, args) => args.ReplaceService<DbProviderServices>(
//                (s, _) => new CachingProviderServices(s, transactionHandler,
//                  cachingPolicy));

//            // this line is for Efprof
//            //AddDependencyResolver(new ProfiledDbDependencyResolver(this));
//		}
//	}

//    public class ProfiledDbDependencyResolver : IDbDependencyResolver
//    {
//        private readonly IDbDependencyResolver _rootResolver;

//#if DEBUG
//        public static HashSet<string> types = new HashSet<string>();
//#endif

//        public ProfiledDbDependencyResolver(DbConfiguration originalDbConfiguration)
//        {
//            // Get the original resolver
//            var internalConfigProp = originalDbConfiguration.GetType().GetProperty("InternalConfiguration", BindingFlags.Instance | BindingFlags.NonPublic);
//            var internalConfig = internalConfigProp.GetValue(originalDbConfiguration, null);
//            var rootResolverProp = internalConfig.GetType().GetProperty("RootResolver", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
//            _rootResolver = (IDbDependencyResolver)rootResolverProp.GetValue(internalConfig, null);
//        }

//        public object GetService(Type type, object key)
//        {
//#if DEBUG
//            types.Add(type.Name);
//#endif
            
//            if (type == typeof(IDbProviderFactoryResolver))
//            {
//                var innerFactoryService = (IDbProviderFactoryResolver)_rootResolver.GetService(type, key);
//                return new ProfiledDbProviderFactoryService(innerFactoryService);
//            }

//            if (type == typeof(DbProviderServices))
//            {
//                var inner = (DbProviderServices)_rootResolver.GetService(type, key);
//                var appender = new EntityFrameworkAppender(typeof(SqlProviderServices).Name);
//                var profiledDbProviderServicesType = EntityFrameworkProfilerInternal.CompiledAssembly.GetType("HibernatingRhinos.Profiler.Appender.EntityFramework.ProfiledDbProviderServices");
//                if (profiledDbProviderServicesType == null)
//                    throw new InvalidOperationException("Could not get the profiled DbProviderServices.");
//                return Activator.CreateInstance(profiledDbProviderServicesType, new object[] { inner, appender });
//            }

//            if (type == typeof(MigrationSqlGenerator))
//            {
//                if (_rootResolver.GetService(type, key) is SqlServerMigrationSqlGenerator)
//                {
//                    return new ProfiledMigrationSqlGenerator();
//                }
//            }

//            return null;
//        }

//        public IEnumerable<object> GetServices(Type type, object key)
//        {
//#if DEBUG
//            types.Add(type.Name);
//#endif

//            if (type == typeof(IDbProviderFactoryResolver))
//            {
//                var innerFactoryService = (IDbProviderFactoryResolver)_rootResolver.GetService(type, key);
//                return new[] {new ProfiledDbProviderFactoryService(innerFactoryService)};
//            }

//            if (type == typeof(DbProviderServices))
//            {
//                var inner = (DbProviderServices)_rootResolver.GetService(type, key);
//                var appender = new EntityFrameworkAppender(typeof(SqlProviderServices).Name);
//                var profiledDbProviderServicesType = EntityFrameworkProfilerInternal.CompiledAssembly.GetType("HibernatingRhinos.Profiler.Appender.EntityFramework.ProfiledDbProviderServices");
//                if (profiledDbProviderServicesType == null)
//                    throw new InvalidOperationException("Could not get the profiled DbProviderServices.");
//                return new[] {Activator.CreateInstance(profiledDbProviderServicesType, new object[] { inner, appender })};
//            }

//            if (type == typeof(MigrationSqlGenerator))
//            {
//                if (_rootResolver.GetService(type, key) is SqlServerMigrationSqlGenerator)
//                {
//                    return new[] {new ProfiledMigrationSqlGenerator()};
//                }
//            }
//            return null;
//        }
//    }

//    public class ProfiledDbProviderFactoryService : IDbProviderFactoryResolver
//    {
//        private readonly IDbProviderFactoryResolver _innerFactoryService;

//        public ProfiledDbProviderFactoryService(IDbProviderFactoryResolver innerFactoryService)
//        {
//            this._innerFactoryService = innerFactoryService;
//        }

//        public DbProviderFactory ResolveProviderFactory(DbConnection connection)
//        {
//            if (connection is ProfiledConnection)
//            {
//                var connectionType = connection.GetType();
//                if (connectionType.IsGenericType)
//                {
//                    var innerProviderFactory = connectionType.GetGenericArguments()[0];
//                    var profiledDbProviderFactory = EntityFrameworkProfilerInternal.CompiledAssembly.GetType("HibernatingRhinos.Profiler.Appender.EntityFramework.ProfiledDbProviderFactory`1").MakeGenericType(innerProviderFactory);
//                    return (DbProviderFactory)Activator.CreateInstance(profiledDbProviderFactory);
//                }
//            }

//            if (connection is EntityConnection)
//                return _innerFactoryService.ResolveProviderFactory(connection);

//            throw new InvalidOperationException("Should have ProfiledConnection but got " + connection.GetType().FullName + ".If you got here, you probably need to modify the above code in order to satisfy the requirements of your application. This code indented to support EF 6 alpha 2.");
//        }
//    }

//    public class ProfiledMigrationSqlGenerator : SqlServerMigrationSqlGenerator
//    {
//        protected override DbConnection CreateConnection()
//        {
//            return DbProviderFactories.GetFactory("System.Data.SqlClient").CreateConnection();
//        }
//    }
//}