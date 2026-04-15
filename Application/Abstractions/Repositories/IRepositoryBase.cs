namespace Application.Abstractions.Repositories;

public interface IRepositoryBase<TModel, TId>
{
    Task<TModel?> CreateAsync(TModel model, CancellationToken ct = default);
    Task<bool> UpdateAsync(TId id, TModel updatedModel, CancellationToken ct = default);
    Task<bool> DeleteAsync(TId id, CancellationToken ct = default);

    Task<TModel?> GetByIdAsync(TId id, CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken ct = default);
}
