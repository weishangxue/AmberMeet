namespace AmberMeet.Domain.Data
{
    public abstract class AbstractRepository
    {
        protected AbstractRepository()
        {
            FactoryContext = UnitOfWork.FactoryContext;
        }

        protected AmberMeetDataContext FactoryContext { get; }

        protected AmberMeetDataContext DataContext
        {
            get { return UnitOfWork.DataContext; }
        }
    }
}