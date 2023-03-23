using Budget.Domain.SeedWork;

namespace Budget.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly BudgetContext _context;

    public UnitOfWork(BudgetContext context)
    {
        _context = context;
    }

    void IUnitOfWork.Commit()
    {
        _context.SaveChanges();
    }
}