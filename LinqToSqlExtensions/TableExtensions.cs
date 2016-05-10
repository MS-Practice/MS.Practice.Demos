using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Linq;

namespace LinqToSqlExtensions
{
    public static class TableExtensions
    {
        public static int Delete<TEntity>(this Table<TEntity> table, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            string tableName = table.Context.Mapping.GetTable(typeof(TEntity)).TableName;
            string command = String.Format("DELETE FROM {0}", tableName);
            ConditionBuilder conditionBuilder = new ConditionBuilder();

            return table.Context.ExecuteCommand(command, conditionBuilder.Arguments);
        }
    }
}
