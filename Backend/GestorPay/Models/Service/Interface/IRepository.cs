using GestorPay.Helper;

namespace GestorPay.Models.Service.Interface
{
    public interface IRepository
    {
        int Save();
        Task<int> SaveAsync(bool defaultValues = true);
        void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity;
        void Insert<TEntity>(TEntity entity) where TEntity : BaseEntity;
        IQueryable<TEntity> Select<TEntity>() where TEntity : BaseEntity;
    }
}
