using ApplicationCore.Contracts.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfRepository<T> : IRespository<T> where T: class
{
    protected readonly MovieShopDbContext _dbContext;

    
    public EfRepository(MovieShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public virtual async Task<T> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<T> Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {

        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return entity;
    }
}