using Ardalis.Specification;
using GRC.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRC.Core.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);

        Task<List<T>> ListAllAsync();

        Task<List<T>> ListAsync(ISpecification<T> spec);

        Task<T> AddAsync(T entity);

        Task<int> UpdateAsync(T entity);

        Task<int> DeleteAsync(T entity);

        Task<int> CountAsync(ISpecification<T> spec);

        Task<T> FirstAsync(ISpecification<T> spec);

        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
    }
}
