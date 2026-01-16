using Microsoft.EntityFrameworkCore;
using PayPalIntegration.Domain.Interfaces;
using PayPalIntegration.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PayPalIntegration.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly PayHubContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(PayHubContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task Create(T entity)
            => await _dbSet.AddAsync(entity);

        public async Task CreateRange(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public async Task<IList<T>> GetAll()
            => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression)
        => await _dbSet.Where(expression).ToListAsync();

        public async Task<T> Find(int id)
            => await _dbSet.FindAsync(id);

        public void Update(T entity)
            => _dbSet.Update(entity);
    }
}
