using HotelBooking.DAL.Contexts;
using HotelBooking.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.DAL.Repositories
{
    public class GenericRepository<TEntity, TId>
            : IGenericRepository<TEntity, TId>
            where TEntity : class, IBaseEntity<TId>
            where TId : notnull
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                var result = await _context.SaveChangesAsync();
                return result != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                var result = await _context.SaveChangesAsync();
                return result != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(TId id)
        {
            try
            {
                return await _context.Set<TEntity>().FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                entity.UpdatedDate = DateTime.UtcNow;
                _context.Set<TEntity>().Update(entity);
                var result = await _context.SaveChangesAsync();
                return result != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
