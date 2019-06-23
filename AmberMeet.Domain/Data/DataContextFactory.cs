using System.Threading;
using System.Web;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Domain.Data
{
    public static class DataContextFactory
    {
        private const string DataContextKey = "AmberMeetDataContext";

        public static AmberMeetDataContext CreateDataContext()
        {
            var connectionString = ConfigHelper.Conn;

            var httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                if (httpContext.Items[DataContextKey] == null)
                {
                    httpContext.Items[DataContextKey] = new AmberMeetDataContext(connectionString);
                }

                return httpContext.Items[DataContextKey] as AmberMeetDataContext;
            }

            var localDataStoreSlot = Thread.GetNamedDataSlot(DataContextKey);

            var dataContext = Thread.GetData(localDataStoreSlot);

            if (dataContext == null)
            {
                dataContext = new AmberMeetDataContext(connectionString);

                Thread.SetData(localDataStoreSlot, dataContext);
            }

            return dataContext as AmberMeetDataContext;
        }
    }
}