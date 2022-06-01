

using AROBlog.Data.Models;
using AROBlog.IDAL;
using Microsoft.EntityFrameworkCore;

namespace AROBlog.DAL
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity, new()
    {
        private readonly BlogContext _blogContext;
        public BaseService(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task CreateAsync(T model, bool saved = true)
        {
            _blogContext.Set<T>().Add(model);
            if (saved) await _blogContext.SaveChangesAsync();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task RemoveAsync(Guid id, bool saved = true)
        {
            _blogContext.ChangeTracker.AutoDetectChangesEnabled = false;
            var t = new T() { Id = id };
            _blogContext.Entry(t).State = EntityState.Deleted;
            t.IsRemoved = true;
            if (saved)
            {
                await _blogContext.SaveChangesAsync();
                _blogContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
        public async Task RemoveAsync(T model, bool saved = true)
        {
            await RemoveAsync(model.Id, saved);
        }
        /// <summary>
        /// 查
        /// </summary>
        /// <param name="model"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task EditAsync(T model, bool saved = true)
        {
            //禁用变更检查
            _blogContext.ChangeTracker.AutoDetectChangesEnabled = false;
            _blogContext.Entry(model).State = EntityState.Modified;
            if (saved)
            {
                await _blogContext.SaveChangesAsync();
                _blogContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
        public async Task Save()
        {
            await _blogContext.SaveChangesAsync();
            _blogContext.ChangeTracker.AutoDetectChangesEnabled = true;
        }
        /// <summary>
        /// 返回所有未被删除的数据（没有真的执行）
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAllAsync()
        {
            return _blogContext.Set<T>().Where(m => !m.IsRemoved).AsNoTracking();

        }
        /// <summary>
        /// id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> GetOneByIdAsync(Guid id)
        {
           return await GetAllAsync().FirstAsync(m=>m.Id==id);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IQueryable<T> GetAllByPageAsync(int pageSize = 10, int pageIndex = 0)
        {
            return GetAllAsync().Skip(pageSize * pageIndex).Take(pageSize);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="asc"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IQueryable<T> GetAllOrderAsync(bool asc = true)
        {
            var datas = GetAllAsync();
            datas=asc?datas.OrderBy(m=>m.CreateTime):datas.OrderByDescending(m=>m.CreateTime);
            return datas;
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public IQueryable<T> GetAllByPageOrderAsync(int pageSize = 10, int pageIndex = 0, bool asc = true)
        {
            return GetAllOrderAsync(asc).Skip(pageSize * pageIndex).Take(pageSize);
        }

        public void Dispose()
        {
            _blogContext.Dispose();
        }

    }
}