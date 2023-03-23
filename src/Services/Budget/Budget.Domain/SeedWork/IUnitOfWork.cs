namespace Budget.Domain.SeedWork;

public interface IUnitOfWork
{
    void Commit();
}