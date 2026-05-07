using BookFlightTickets.Core.Domain.Specifications;
using System.Linq.Expressions;

namespace Villa_API_Project.DataAccess.Reposatory.IReposatory
{
    public interface IReposatory<T> where T:class
    {
           void Create(T model);
        void Update(T model);
        void Delete(T model);

        void RemoveRange(IEnumerable<T> entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec, bool trackchange = false);
        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<int> CountAsync(ISpecification<T>? spec = null);

    }
}
