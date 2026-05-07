using Microsoft.EntityFrameworkCore;

using Villa_API_Project.DataAccess.Data;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;
using BookFlightTickets.Core.Domain.Specifications;

namespace Villa_API_Project.DataAccess.Reposatory
{
    public class Reposatory<T> : IReposatory<T> where T : class
    {

        private Context context;
        internal DbSet<T> Dbset;
        public Reposatory(Context context)
        {
            this.context = context;
            Dbset = context.Set<T>();
        }
        public void Create(T model)
        {
            Dbset.Add(model);
        }

        public void Delete(T model)
        {
            Dbset.Remove(model);
        }

       
        public void Update(T model)
        {
            Dbset.Update(model);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            Dbset.RemoveRange(entity);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await Dbset.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Dbset.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec, bool trackchange = false)
        {

            if (trackchange)
                return await SpecificationEvalutor<T>
                       .GetQuery(Dbset, spec)
                       .FirstOrDefaultAsync();

            return await SpecificationEvalutor<T>
                        .GetQuery(Dbset, spec)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await SpecificationEvalutor<T>
                .GetQuery(Dbset, spec)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await Dbset.AddRangeAsync(entities);
        }


        public async Task<int> CountAsync(ISpecification<T>? spec = null)
        {
            if (spec == null)
            {
                return await Dbset.CountAsync();
            }
            else
            {
                return await SpecificationEvalutor<T>
                            .GetQuery(Dbset, spec)
                            .CountAsync();
            }
        }

    }
}
