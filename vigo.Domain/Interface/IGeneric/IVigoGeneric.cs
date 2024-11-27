using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;

namespace vigo.Domain.Interface.IGeneric
{
    public interface IVigoGeneric<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(IEnumerable<Expression<Func<T, bool>>>? where);
        Task<PagedResultCustom<T>> GetPaging(IEnumerable<Expression<Func<T,bool>>>? where,
                                             Expression<Func<T, string>>? sortString,
                                             Expression<Func<T, decimal>>? sortNumber,
                                             Expression<Func<T, DateTime>>? sortDate,
                                             int pageIndex,
                                             int pageSize,
                                             bool sortDown = false);
        Task<T> GetById(int id);
        Task<T?> GetDetailBy(Expression<Func<T, bool>> where);
        void Create(T entity);
        void CreateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task DeleteRangeById(List<int> ids);
    }
}
