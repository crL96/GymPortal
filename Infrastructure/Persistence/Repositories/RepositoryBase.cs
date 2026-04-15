using Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<TModel, TId, TEntity, TContext>(TContext context) : IRepositoryBase<TModel, TId> where TEntity : class where TContext : DbContext
{
    protected readonly TContext _context = context;
    protected DbSet<TEntity> Set => _context.Set<TEntity>();

    protected abstract TModel ToModel(TEntity entity);
    protected abstract TEntity ToEntity(TModel model);


    public virtual async Task<TModel?> CreateAsync(TModel model, CancellationToken ct = default)
    {
        try
        {
            var entity = ToEntity(model);
            await Set.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return ToModel(entity);
        }
        catch
        {
            return default;
        }
    }

    public virtual async Task<bool> DeleteAsync(TId id, CancellationToken ct = default)
    {
        try
        {
            var entity = await Set.FindAsync([id], ct);
            if (entity == null)
                return false;

            Set.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public virtual async Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await Set.ToListAsync();
        return entities.Select(ToModel).ToList();
    }

    public virtual async Task<TModel?> GetByIdAsync(TId id, CancellationToken ct = default)
    {
        var entity = await Set.FindAsync([id], ct);
        if (entity == null)
            return default;

        return ToModel(entity);
    }

    public virtual async Task<bool> UpdateAsync(TId id, TModel updatedModel, CancellationToken ct = default)
    {
        try
        {
            var entity = await Set.FindAsync([id], ct);
            if (entity == null)
                return default;

            var updatedEntity = ToEntity(updatedModel);

            _context.Entry(entity).CurrentValues.SetValues(updatedEntity);
            await _context.SaveChangesAsync(ct);
            return true;

        }
        catch
        {
            return false;
        }
    }
}