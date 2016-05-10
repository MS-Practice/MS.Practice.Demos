using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace CommomCore
{
    public static class DataContextExtenssions
    {
        public static void OpenConnection(this DataContext dataContext) {
            if (dataContext.Connection.State == ConnectionState.Closed) {
                dataContext.Connection.Open();
            }
        }
        public static IAsyncResult BeginExecuteQuery(this DataContext dataContext, IQueryable query, bool withNoLock, AsyncCallback callback, object asyncState)
        {
            SqlCommand command = (SqlCommand)dataContext.GetCommand(query,withNoLock);
            dataContext.OpenConnection();
            AsyncResult<DbDataReader> asyncResult = new AsyncResult<DbDataReader>(asyncState);
            command.BeginExecuteNonQuery(ar =>
            {
                try
                {
                    asyncResult.Result = command.EndExecuteReader(ar);
                }
                catch
                {
                    throw asyncResult.Exception;
                }
                finally {
                    asyncResult.Complete();
                    if (callback != null) callback(asyncResult);
                }
            }, null);
            return asyncResult;
        }
        public static List<T> EndExecuteQuery<T>(this DataContext dataContext, IAsyncResult ar) {
            AsyncResult<DbDataReader> asyncResult = (AsyncResult<DbDataReader>)ar;
            if (!asyncResult.IsCompleted) {
                asyncResult.AsyncWaitHandle.WaitOne();
            }
            if (asyncResult.Exception != null) {
                throw asyncResult.Exception;
            }
            using (DbDataReader reader = asyncResult.Result) {
                return dataContext.Translate<T>(reader).ToList();
            }
        }

        public static SqlCommand GetCommand(this DataContext dataContext, IQueryable query, bool widthNoLock) {
            SqlCommand command = (SqlCommand)dataContext.GetCommand(query);
            if (widthNoLock) {
                command.CommandText = AddWithNoLock(command.CommandText);
            }
            return command;
        }

        private static Regex s_withNoLockRegex = new Regex(@"] AS \[t\d+\])", RegexOptions.Compiled);
        public static string AddWithNoLock(string CommandText) {
            IEnumerable<Match> matchs = s_withNoLockRegex.Matches(CommandText).Cast<Match>().OrderByDescending(m => m.Index);
            foreach (Match m in matchs)
            {
                int splitIndex = m.Index + m.Value.Length;
                CommandText = CommandText.Substring(0, splitIndex) + " WITH (NOLOCK)" + CommandText.Substring(splitIndex);
            }
            return CommandText;
        }
    }
}
