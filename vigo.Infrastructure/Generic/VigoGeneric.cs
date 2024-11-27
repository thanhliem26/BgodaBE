using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Domain.Interface.IGeneric;
using vigo.Infrastructure.DBContext;
using System.Linq.Dynamic.Core;

namespace vigo.Infrastructure.Generic
{
    public class VigoGeneric<T> : IVigoGeneric<T> where T : class
    {
        protected readonly VigoDatabaseContext _context;
        public VigoGeneric(VigoDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAll(IEnumerable<Expression<Func<T, bool>>>? where)
        {
            var query = _context.Set<T>().AsQueryable();
            if (where != null)
            {
                foreach (var expression in where)
                {
                    query = query.Where(expression);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<PagedResultCustom<T>> GetPaging(IEnumerable<Expression<Func<T, bool>>>? where,
                                                          Expression<Func<T, string>>? sortString,
                                                          Expression<Func<T, decimal>>? sortNumber,
                                                          Expression<Func<T, DateTime>>? sortDate,
                                                          int pageIndex,
                                                          int pageSize,
                                                          bool sortDown = false)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();
            if (where != null) {
                foreach (var expression in where)
                {
                    query = query.Where(expression);
                }
            }
            if (sortString != null)
            {
                query = sortDown ? query.OrderByDescending(sortString) : query.OrderBy(sortString);
            }
            if (sortNumber != null)
            {
                query = sortDown ? query.OrderByDescending(sortNumber) : query.OrderBy(sortNumber);
            }
            if (sortDate != null)
            {
                query = sortDown ? query.OrderByDescending(sortDate) : query.OrderBy(sortDate);
            }

            var totalRecords = await query.CountAsync();
            var result = await query.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PagedResultCustom<T>(result, totalRecords, pageIndex, pageSize);
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void CreateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task DeleteRangeById(List<int> ids)
        {
            List<T> list = new List<T>();
            foreach (var id in ids)
            {
                var data = await _context.Set<T>().FindAsync(id);
                if (data != null)
                {
                    list.Add(data);
                }
            }
            _context.Set<T>().RemoveRange(list);
        }

        public async Task<T> GetById(int id)
        {
            var data = await _context.Set<T>().FindAsync(id);
            if (data == null)
            {
                throw new Exception("Dữ liệu không tồn tại");
            }
            return data;
        }

        public async Task<T?> GetDetailBy(Expression<Func<T, bool>> where)
        {
            var query = _context.Set<T>().AsQueryable();
            query = query.Where(where);
            return await query.FirstOrDefaultAsync();
        }
    }
}
