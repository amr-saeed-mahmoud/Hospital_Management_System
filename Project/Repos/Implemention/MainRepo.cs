using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Project.Repos.Abstract;

namespace Project.Repos.Implemention;

public class MainRepo<T> : IMainRepo<T> where T : class
{
    private readonly AppDbContext _db;
    private readonly DbSet<T> _dbSet;

    public MainRepo(AppDbContext context)
    {
        _db = context;
        _dbSet = _db.Set<T>();
    }

    public async Task<int> AddNew(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _db.SaveChangesAsync();

        return (int)typeof(T).GetProperty("Id")?.GetValue(entity)!;
    }
    public async Task<bool> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        int IsDeleted = 0;
        if (entity != null)
        {
            _dbSet.Remove(entity);
            IsDeleted = await _db.SaveChangesAsync();
        }
        return IsDeleted > 0;
    }

    public async Task<T?> FindByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindByExpression(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.Where(expression).ToListAsync();
    }

    public async Task<bool> IsExist(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        return query;
    }
    
}
