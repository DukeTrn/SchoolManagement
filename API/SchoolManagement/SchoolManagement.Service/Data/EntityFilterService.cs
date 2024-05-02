using Microsoft.EntityFrameworkCore;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Common.Extensions;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention.Data;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace SchoolManagement.Service
{
    public sealed class EntityFilterService<T> : IEntityFilterService<T>
    {
        static readonly MethodInfo containsMethod = typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(p => p.Name == nameof(Enumerable.Contains) && p.GetParameters().Length == 2)
            .First();

        static readonly MethodInfo upperMethod = typeof(string).GetMethods()
            .Where(p => p.Name == nameof(string.ToUpper) && p.GetParameters().Length == 0)
            .First();

        static readonly MethodInfo efLikeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            BindingFlags.Public | BindingFlags.Static,
            new[] { typeof(DbFunctions), typeof(string), typeof(string) })!;

        private readonly IReadOnlyDictionary<string, MemberExpression> propExps;
        private readonly IReadOnlyDictionary<string, Type> propTypes;
        private readonly ParameterExpression param;
        private readonly Type type;
        public EntityFilterService()
        {
            type = typeof(T);
            param = Expression.Parameter(type, "p");

            var expDict = new Dictionary<string, MemberExpression>();
            var propDict = new Dictionary<string, Type>();
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.PropertyType.IsConstructedGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    expDict[prop.Name] = Expression.Property(Expression.Property(param, prop), "Value");
                    propDict[prop.Name] = prop.PropertyType.GenericTypeArguments[0];
                }
                else
                {
                    expDict[prop.Name] = Expression.Property(param, prop);
                    propDict[prop.Name] = prop.PropertyType;
                }
            }

            propExps = expDict;
            propTypes = propDict;
        }
        public IQueryable<T> BuildFilterQuery(IQueryable<T> query, IEnumerable<FilterItemModel> filters)
        {
            return BuildFilterQuery(query, new FilterModel() { And = filters });
        }

        public IQueryable<T> BuildFilterQuery(in IQueryable<T> query, FilterModel filter)
        {
            var result = query;
            if (filter.And.NotEmpty())
            {
                foreach (var item in filter.And)
                {
                    var exp = CreateQuery(item.KeyName, item.Values);
                    result = result.Where(exp);
                }
            }

            foreach (var item in filter.AndOp)
            {
                var exp = CreateExpression(item.KeyName!, item.Values, item.Operator);
                var lambda = ToLambda<bool>(param, exp);
                result = result.Where(lambda);
            }

            //If keyname is empty, system will query every column in special entity with search text.
            var orItemsForAllColumns = filter.Or.Where(i => string.IsNullOrEmpty(i.KeyName));
            Expression? or = null;
            foreach (var item in orItemsForAllColumns)
            {
                foreach (var kv in propTypes)
                {
                    if (kv.Value != item.KeyType)
                    {
                        continue;
                    }

                    var exp = CreateExpression(kv.Key, item.Values, item.Operator);
                    if (or == null)
                    {
                        or = exp;
                    }
                    else
                    {
                        or = Expression.OrElse(or, exp);
                    }
                }
                var lambda = ToLambda<bool>(param, or!);
                result = result.Where(lambda);
            }
            //If keyname is specified column, system will build query condition with columns only.
            var orItemsForSpecialColumns = filter.Or.Where(i => !string.IsNullOrEmpty(i.KeyName));
            or = null;
            foreach (var item in orItemsForSpecialColumns)
            {
                var exp = CreateExpression(item.KeyName, item.Values, item.Operator);
                if (or == null)
                {
                    or = exp;
                }
                else
                {
                    or = Expression.OrElse(or, exp);
                }
            }
            if (null != or)
            {
                var orLambda = ToLambda<bool>(param, or!);
                result = result.Where(orLambda);
            }
            return result;
        }

        public async ValueTask<List<TKey>> GetColumnValuesAsync<TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector)
        {
            var list = await query.Select(keySelector).Distinct().ToListAsync();
            return list;
        }
        public async ValueTask<System.Collections.IList> GetColumnValuesAsync(IQueryable<T> query, string name)
        {
            var (param, member, type) = GetProperty(name);

            if (type == typeof(int))
            {
                var exp = ToLambda<int>(param, member);
                return await GetColumnValuesAsync(query, exp);
            }
            else if (type == typeof(long))
            {
                var exp = ToLambda<long>(param, member);
                return await GetColumnValuesAsync(query, exp);
            }
            else if (type == typeof(float))
            {
                var exp = ToLambda<float>(param, member);
                return await GetColumnValuesAsync(query, exp);
            }
            else if (type == typeof(double))
            {
                var exp = ToLambda<double>(param, member);
                return await GetColumnValuesAsync(query, exp);
            }
            else if (type == typeof(bool))
            {
                var exp = ToLambda<bool>(param, member);
                return await GetColumnValuesAsync(query, exp);
            }
            else if (type == typeof(DateTime))
            {
                var exp = ToLambda<DateTime>(param, member);
                return await GetColumnValuesAsync(query, exp);
            }
            else if (type == typeof(string))
            {
                var exp = ToLambda<string>(param, member);
                return await GetColumnValuesAsync(query, exp);
            }
            throw new Exception($"Unknown Type {type}");
        }

        private Expression<Func<T, bool>> CreateQuery(string propName, IEnumerable<string> searchValues)
        {
            var list = CreateList(propTypes[propName], searchValues);
            var exp = CreateExpression(propName, list);

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private Expression CreateExpression(string propName, IList list, FilterOperator op = FilterOperator.In)
        {
            var prop = propExps[propName];
            var type = propTypes[propName];
            if (op == FilterOperator.In)
            {
                if (list.Count == 1)
                {
                    return Expression.Equal(prop, Expression.Constant(list[0]));
                }
                else
                {
                    var genericMethod = containsMethod.MakeGenericMethod(type);
                    var expContains = Expression.Call(genericMethod, Expression.Constant(list), propExps[propName]);
                    return expContains;
                }
            }
            else if (op == FilterOperator.NotIn)
            {
                if (list.Count == 1)
                {
                    return Expression.NotEqual(prop, Expression.Constant(list[0]));
                }
                else
                {
                    var genericMethod = containsMethod.MakeGenericMethod(type);
                    var expContains = Expression.Call(genericMethod, Expression.Constant(list), propExps[propName]);
                    return Expression.Not(expContains);
                }
            }
            else if (op == FilterOperator.Range)
            {
                if (list.Count != 2)
                {
                    throw LogicException.CommonError("FilterOperator.Range should only have 2 values");
                }
                var exp1 = Expression.LessThanOrEqual(Expression.Constant(list[0]), prop);
                var exp2 = Expression.LessThan(prop, Expression.Constant(list[1]));
                var exp = Expression.AndAlso(exp1, exp2);

                return exp;
            }
            else if (op == FilterOperator.Contains)
            {
                if (list.Count != 1)
                {
                    throw LogicException.CommonError("FilterOperator.StartWith should only have 1 value");
                }
                var upperExpress = Expression.Call(prop, upperMethod);
                var like = Expression.Call(efLikeMethod, Expression.Constant(EF.Functions), upperExpress, Expression.Constant($"%{(list[0] as string)?.ToUpper()}%"));
                return like;
            }
            else if (op == FilterOperator.NotNull)
            {
                return Expression.NotEqual(prop, Expression.Constant(null, typeof(object)));
            }
            throw new NotSupportedException($"Unsupport filter operator {op}");
        }

        private (ParameterExpression, MemberExpression, Type) GetProperty(string propName)
        {
            return (param, propExps[propName], propTypes[propName]);
        }

        private static Expression<Func<T, TKey>> ToLambda<TKey>(ParameterExpression param, Expression member) =>
            Expression.Lambda<Func<T, TKey>>(member, param);

        private static IList CreateList(Type propType, IEnumerable<string> reqValues)
        {
            // Do not use Convert.ChangeType method here
            // Becuase it will box struct to object and use extra memory and make low performence
            if (propType == typeof(int))
            {
                return CreateGenericList(reqValues, s => int.Parse(s));
            }
            else if (propType == typeof(long))
            {
                return CreateGenericList(reqValues, s => long.Parse(s));
            }
            else if (propType == typeof(float))
            {
                return CreateGenericList(reqValues, s => float.Parse(s));
            }
            else if (propType == typeof(double))
            {
                return CreateGenericList(reqValues, s => double.Parse(s));
            }
            else if (propType == typeof(bool))
            {
                return CreateGenericList(reqValues, s => bool.Parse(s));
            }
            else if (propType == typeof(DateTime))
            {
                return CreateGenericList(reqValues, s => s.ConvertToDateTime()!.Value);
            }
            else if (propType == typeof(string))
            {
                return reqValues.ToList();
            }
            throw new ArgumentException($"Unknown Type {propType}");
        }

        private static List<TItem> CreateGenericList<TItem>(IEnumerable<string> reqValues, Func<string, TItem> parser)
        {
            var list = new List<TItem>();
            foreach (var item in reqValues)
            {
                list.Add(parser(item));
            }
            return list;
        }
    }
}