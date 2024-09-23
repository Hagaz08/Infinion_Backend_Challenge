using Infinion.Core.Abstractions;
using Infinion.Data.Context;

namespace Infinion.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        { 
            return await _context.SaveChangesAsync();
        }

    }
}

