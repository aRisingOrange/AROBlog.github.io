using AROBlog.Data.Models;

namespace AROBlog.IDAL
{
    //IDisposable
    //此接口的主要用途是释放非托管资源。 当不再使用该对象时，垃圾回收器会自动释放分配给托管对象的内存。 
    public interface IBaseService<T>:IDisposable where T : BaseEntity
    {
        Task CreateAsync(T model, bool saved = true);
        Task RemoveAsync(Guid id ,bool saved=true);
        Task EditAsync(T model, bool saved = true);
        Task RemoveAsync(T model, bool saved = true);
        Task Save();
        Task<T> GetOneByIdAsync(Guid id);
        IQueryable<T> GetAllAsync();
        IQueryable<T> GetAllByPageAsync(int pageSize = 10, int pageIndex = 0);
        IQueryable<T> GetAllOrderAsync(bool asc = true);
        IQueryable<T> GetAllByPageOrderAsync(int pageSize = 10, int pageIndex = 0, bool asc = true);
    }
}