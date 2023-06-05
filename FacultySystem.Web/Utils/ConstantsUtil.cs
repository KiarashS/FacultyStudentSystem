using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace ContentManagementSystem.Web.Utils
{
    public static class ConstantsUtil
    {
        public const string AdminRole = "admin";
        public const string ProfessorRole = "professor";
        public const string AdminProfessorRoles = "admin,professor";
        public const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
        public const int ResponseStreamTimeout = 35000;
        public static Task<T> WithTimeout<T>(this Task<T> task, int duration)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var b = task.Wait(duration);
                    return b ? task.Result : default(T);
                }
                catch (AggregateException e)
                {
                    return default(T);
                }
            });
        }
    }
}