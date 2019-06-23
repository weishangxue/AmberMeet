using System;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Domain.Data
{
    internal class UnitOfWork : IUnitOfWork
    {
        static UnitOfWork()
        {
            FactoryContext = DataContextFactory.CreateDataContext();
        }

        public static AmberMeetDataContext FactoryContext { get; }

        public static AmberMeetDataContext DataContext
        {
            get { return new AmberMeetDataContext(ConfigHelper.Conn); }
        }

        public void Commit()
        {
            FactoryContext.SubmitChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                FactoryContext.Dispose();
        }
    }
}