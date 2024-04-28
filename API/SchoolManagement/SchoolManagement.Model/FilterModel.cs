using SchoolManagement.Common.Enum;
using System.Linq.Expressions;

namespace SchoolManagement.Model
{
    public sealed class FilterModel
    {
        public IEnumerable<FilterItemModel> And { get; set; } = null!;
        public List<FilterOperatorItemModel> AndOp { get; set; } = new();
        public List<FilterOperatorItemModel> Or { get; set; } = new();

        public FilterModel AddOr<T>(List<T> values, FilterOperator op = FilterOperator.In)
        {
            var item = new FilterOperatorItemModel()
            {
                Operator = op,
                Values = values,
                KeyType = typeof(T)
            };
            Or.Add(item);

            return this;
        }

        public FilterModel AddAnd<TEntity, TProp>(Expression<Func<TEntity, TProp>> prop, List<TProp> values, FilterOperator op = FilterOperator.In)
        {
            var item = new FilterOperatorItemModel()
            {
                Operator = op,
                KeyName = ((MemberExpression)prop.Body).Member.Name,
                Values = values,
                KeyType = typeof(TProp)
            };
            AndOp.Add(item);

            return this;
        }

        public FilterModel AddAnd<TEntity, TProp>(Expression<Func<TEntity, Nullable<TProp>>> prop, List<TProp> values, FilterOperator op = FilterOperator.In) where TProp : struct
        {
            var item = new FilterOperatorItemModel()
            {
                Operator = op,
                KeyName = ((MemberExpression)prop.Body).Member.Name,
                Values = values,
                KeyType = typeof(TProp)
            };
            AndOp.Add(item);

            return this;
        }
    }
}
