using Budget.Domain.SeedWork;

using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly BudgetContext _context;

    public UnitOfWork(BudgetContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}