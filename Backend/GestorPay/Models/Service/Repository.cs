using GestorPay.Context;
using GestorPay.Helper;
using GestorPay.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestorPay.Models.Service
{
    public class Repository : IRepository
    {
        private readonly GestorPayContext _context;

        public Repository(GestorPayContext context)
        {
            _context = context;
        }

        public void Insert<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _context.Set<TEntity>().Add(entity);
        }

        public int Save()
        {
            SetDefaultValues(_context);
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync(bool defaultValues = true)
        {
            if (defaultValues)
                SetDefaultValues(_context);

            return await _context.SaveChangesAsync();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> Select<TEntity>() where TEntity : BaseEntity
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        private void SetDefaultValues(DbContext context)
        {
            context
                .ChangeTracker
                .Entries()
                .Where(p => p.State != EntityState.Unchanged)
                .ToList()
                .ForEach(p =>
                {
                    if (p.State == EntityState.Added)
                        p.Entity.GetType().GetProperty("CreationDate")?.SetValue(p.Entity, DateTime.Now);

                    p.Entity.GetType().GetProperty("LastUpdate")?.SetValue(p.Entity, DateTime.Now);
                    p.Entity.GetType().GetProperty("UpdateDate")?.SetValue(p.Entity, DateTime.Now);
                    p.Entity.GetType().GetProperty("LastUpdateDate")?.SetValue(p.Entity, DateTime.Now);
                    
                });
        }
    }
}
