using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace SchoolManagement.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool NotEmpty<T>(this IEnumerable<T>? enumerable)
        {
            if (enumerable == null || !enumerable.Any())
            {
                return false;
            }
            return true;
        }
        public static bool IsEmpty<T>(this IList<T>? list)
        {
            if (list == null || !list.Any())
            {
                return true;
            }
            return false;
        }
        public static string JoinWithComma(this IEnumerable<string>? str)
        {
            if (str is null || !str.Any())
            {
                return string.Empty;
            }

            return string.Join(',', str);
        }
        public static IEnumerable<T> SelectIf<T>(this IEnumerable<T> values, bool condition, Func<T, T> sel)
        {
            if (condition)
            {
                return values.Select(sel);
            }
            return values;
        }
        public static IEnumerable<T> SkipLastIf<T>(this IEnumerable<T> values, bool condition, int count)
        {
            if (condition)
            {
                return values.SkipLast(count);
            }
            return values;
        }
        public static DataTable ToDataTable<T>(IEnumerable<T> ts)
        {
            var type = typeof(T);
            var dict = new Dictionary<string, PropertyInfo>();
            var dt = new DataTable();

            foreach (var prop in type.GetProperties())
            {
                var attr = prop.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                var colName = attr?.Name ?? prop.Name;

                dt.Columns.Add(colName, prop.PropertyType);
                dict.Add(colName, prop);
            }

            foreach (var t in ts)
            {
                var dr = dt.NewRow();
                foreach (var kv in dict)
                {
                    dr[kv.Key] = kv.Value.GetValue(t);
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
