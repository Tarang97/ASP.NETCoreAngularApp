using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repositories;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _ctx;
        private Hashtable _repositories;
        public UnitOfWork(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<int> Complete()
        {
            return await _ctx.SaveChangesAsync();
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if(!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                // Rather than using or creating an instance of our context when we create our repository instance
                // we're gonna be passing in the context to our units of work owns as a parameter into the repository
                // that we're creating their.
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(
                    typeof(TEntity)), _ctx);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>) _repositories[type];
        }
    }
}