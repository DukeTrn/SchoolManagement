using SchoolManagement.Model;
using System.Linq.Expressions;

namespace SchoolManagement.Service.Intention.Data
{
    public interface IEntityFilterService<T>
    {
        IQueryable<T> BuildFilterQuery(IQueryable<T> query, IEnumerable<FilterItemModel> filters);
        IQueryable<T> BuildFilterQuery(in IQueryable<T> query, FilterModel filter);
        ValueTask<List<TKey>> GetColumnValuesAsync<TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector);
        ValueTask<System.Collections.IList> GetColumnValuesAsync(IQueryable<T> query, string name);
    }
}
