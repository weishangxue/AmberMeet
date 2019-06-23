namespace AmberMeet.Domain.Data
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}