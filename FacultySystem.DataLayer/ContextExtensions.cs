using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ContentManagementSystem.DataLayer
{
    public static class ContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            var sql = context.CreateObjectSet<T>().ToTraceString();
            var regex = new Regex("FROM (?<table>.*) AS");
            var match = regex.Match(sql);
            var table = match.Groups["table"].Value;
            return table
                    .Replace("`", string.Empty)
                    .Replace("[", string.Empty)
                    .Replace("]", string.Empty)
                    .Replace("dbo.", string.Empty)
                    .Trim();
        }

        private static bool HasUniqueIndex(this DbContext context, string tableName, string indexName)
        {
            var sql = "SELECT count(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS where table_name = '"
                      + tableName + "' and CONSTRAINT_NAME = '" + indexName + "'";
            var result = context.Database.SqlQuery<int>(sql).FirstOrDefault();
            return result > 0;
        }

        private static void createUniqueIndex(this DbContext context, string tableName, string fieldName)
        {
            var indexName = "IX_Unique_" + tableName + "_" + fieldName;
            if (HasUniqueIndex(context, tableName, indexName))
                return;

            var sql = "ALTER TABLE [" + tableName + "] ADD CONSTRAINT [" + indexName + "] UNIQUE ([" + fieldName + "])";
            context.Database.ExecuteSqlCommand(sql);
        }

        public static void CreateUniqueIndex<TEntity>(this DbContext context, Expression<Func<TEntity, object>> fieldName) where TEntity : class
        {
            //var field = ((MemberExpression)fieldName.Body).Member.Name;
            var field = GetCorrectPropertyName(fieldName);
            createUniqueIndex(context, context.GetTableName<TEntity>(), field);
        }

        public static string GetCorrectPropertyName<T>(Expression<Func<T, Object>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                return ((MemberExpression)expression.Body).Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }
    }
}
