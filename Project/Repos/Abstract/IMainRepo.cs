using System.Linq.Expressions;

namespace Project.Repos.Abstract;

public interface IMainRepo<T> where T : class
{
    Task<int> AddNew(T entity);                 // Adds a new entity
    Task<bool> UpdateAsync(T entity);                // Updates an existing entity
    Task<bool> DeleteAsync(int id);                        // Deletes an entity by its ID
    Task<T?> FindByIdAsync(int id);                  // Finds an entity by its ID
    Task<IEnumerable<T>> GetAllAsync();              // Retrieves all entities
    Task<IEnumerable<T>> FindByExpression(Expression<Func<T, bool>> expression); // Finds entities by condition
    Task<bool> IsExist(Expression<Func<T, bool>> expression); // check if entity is exists or not
    IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
}