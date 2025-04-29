using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repository;
using Core.Specification;
using Microsoft.EntityFrameworkCore;
using Reposatory.Data;

namespace Reposatory.Repository
{
    public class GenericReposatory<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericReposatory(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
          await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

        }


       

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }

      

        public async Task<IEnumerable<T>> GetAllAsync()
        {
         
           return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithSpecification(Ispecification<T> spec)
        {
          return await Getquery(spec).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdWithSpecification(Ispecification<T> spec)
        {
          return await Getquery(spec).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

       

        public IQueryable<T> Getquery(Ispecification<T>spec)
        {
            return Evouation<T>.ApplySpecification(_dbContext.Set<T>(), spec);
        }

        
    }
}
