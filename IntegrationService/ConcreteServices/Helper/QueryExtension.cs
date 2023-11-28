using GridShared.Filtering;
using IntegrationService.Models.Product;
using IntegrationService.ViewModels.FieldProductsViewModels;
using LinqToDB;
using LinqToDB.Expressions;
using LinqToDB.Linq;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static LinqToDB.Reflection.Methods.LinqToDB.Insert;
using static OfficeOpenXml.ExcelErrorValue;

namespace IntegrationService.ConcreteServices.Helper
{
    public static class QueryExtension
    {
        private static readonly MethodInfo SetMethodInfo = MemberHelper
         .MethodOf<object>(o => ((IUpdatable<object>)null).Set(null, 0))
         .GetGenericMethodDefinition();

        private static readonly MethodInfo SqlPropertyMethodInfo = typeof(Sql).GetMethod("Property")
           .GetGenericMethodDefinition();

        public enum FieldSource
        {
            Propety,
            Column
        }

        public static Func<ParameterExpression, KeyValuePair<string, object>, Expression> GetFieldFunc(FieldSource fieldSource)
        {
            switch (fieldSource)
            {
                case FieldSource.Propety:
                    return GetPropertyExpression;
                case FieldSource.Column:
                    return GetColumnExpression;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldSource), fieldSource, null);
            }
        }
        
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, Dictionary<string, object> sortDictionary)
        {
            var param = Expression.Parameter(typeof(T), "item");
            var query = source.AsQueryable<T>();

            bool isFirstSort = true;

            foreach (var kvp in sortDictionary)
            {
                var sortBy = kvp.Key;
                var sortDirection = kvp.Value as string[];

                var sortExpression = Expression.Lambda<Func<T, object>>(
                    Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

                if (isFirstSort)
                {
                    switch (sortDirection.FirstOrDefault().ToLower())
                    {
                        case "asc":
                            query = query.OrderBy(sortExpression);
                            break;
                        case "desc":
                            query = query.OrderByDescending(sortExpression);
                            break;
                        default:
                            throw new ArgumentException("Invalid sort direction.");
                    }

                    isFirstSort = false;
                }
                else
                {
                    switch (sortDirection.FirstOrDefault().ToLower())
                    {
                        case "asc":
                            query = ((IOrderedQueryable<T>)query).ThenBy(sortExpression);
                            break;
                        case "desc":
                            query = ((IOrderedQueryable<T>)query).ThenByDescending(sortExpression);
                            break;
                        default:
                            throw new ArgumentException("Invalid sort direction.");
                    }
                }
            }

            return query;
        }

        public static IQueryable<T> DynamicFilterByValues<T>(this IQueryable<T> source,
          IEnumerable<KeyValuePair<string, object>> values,
          Func<ParameterExpression, KeyValuePair<string, object>, Expression> fieldFunc)
        {
            var param = Expression.Parameter(typeof(T));
            var anyPredicates = new List<Expression<Func<T, bool>>>();
            var fieldParameter = Expression.Parameter(typeof(T), "field");
            foreach (var pair in values)
            {
               
                var expressions = new List<Expression>();

                foreach (var part in pair.Value as Dictionary<string, object>)
                {
                    var value = part.Value;
                    var property = Expression.PropertyOrField(fieldParameter, part.Key);

                    Expression comparisonExpression;
                    if (value is Guid guidValue)
                    {
                        var nullableGuid = new Guid?(guidValue);
                        var constant = Expression.Constant(nullableGuid, typeof(Guid?));
                        comparisonExpression = Expression.Equal(property, constant);
                    }
                    else if (value is string stringValue)
                    {
                        var constant = Expression.Constant(stringValue, typeof(string));
                        comparisonExpression = Expression.Equal(property, constant);
                    }
                    else if (value is string[] stringArray)
                    {
                        var fieldExpression = fieldFunc(fieldParameter, part);
                        if (stringArray.Length == 1)
                        {
                            var valueItem = Convert.ChangeType(stringArray.FirstOrDefault(), fieldExpression.Type);
                            var containsMethod = typeof(string).GetMethod("Contains", new[] { fieldExpression.Type });
                            comparisonExpression = Expression.Call(fieldExpression, containsMethod, Expression.Constant(valueItem));
                        }
                        else
                        {
                            var constantExpressions = stringArray.Select(x => Expression.Constant(x));
                            var arrayExpression = Expression.NewArrayInit(typeof(string), constantExpressions);
                            var containsMethod = typeof(Enumerable).GetMethods()
                                .Where(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                                .Single()
                                .MakeGenericMethod(typeof(string));
                            comparisonExpression = Expression.Call(containsMethod, arrayExpression, fieldExpression);
                        }

                        
                    }
                    else
                    {
                        throw new InvalidOperationException("Unsupported type in dictionary.");
                    }

                    expressions.Add(comparisonExpression);
                }

                var anyPredicate = Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(expressions.FirstOrDefault(), expressions.LastOrDefault()),
                    fieldParameter);

                anyPredicates.Add(anyPredicate);
            }

            Expression orPredicate = Expression.Constant(false);
            foreach (var predicate in anyPredicates)
            {
                orPredicate = Expression.OrElse(orPredicate, predicate.Body);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(orPredicate, fieldParameter);
            source = source.Where(lambda);
            return source.AsQueryable();

        }

        public static IQueryable<T> FilterByValues<T>(this IQueryable<T> source,
           IEnumerable<KeyValuePair<string, object>> values,
           Func<ParameterExpression, KeyValuePair<string, object>, Expression> fieldFunc)
        {
            var param = Expression.Parameter(typeof(T));

            foreach (var pair in values)
            {
                var parts = pair.Key.Split('.');
                if (parts[0]=="Fields")
                {
                    var fieldParameter = Expression.Parameter(typeof(FieldPerProductViewModel), "field");
                    var expressions = new List<Expression>();
                    foreach (var part in pair.Value as Dictionary<string,object>)
                    {
                        expressions.Add(Expression.Equal(
                                        Expression.PropertyOrField(fieldParameter, part.Key),
                                         Expression.Constant(part.Value)));

                        
                    }
                    var anyPredicate = Expression.Lambda<Func<FieldPerProductViewModel, bool>>(
                    Expression.AndAlso(
                                expressions.FirstOrDefault(), expressions.LastOrDefault()),
                                    fieldParameter);
                    //var fieldCondition = Expression.Call(
                    //        typeof(Enumerable), "Any", new[] { fieldParameter.Type },
                    //                             Expression.PropertyOrField(param, "Fields"), anyPredicate);
                    var lambda = Expression.Lambda<Func<T, bool>>(
                        anyPredicate,
                        //fieldCondition,
                        param);
                    source = source.Where(lambda);
                }
                var fieldExpression = fieldFunc(param, pair);
                if (fieldExpression != null)
                {
                    var typeValue = pair.Value.GetType();
                    var value = pair.Value as string[];
                    if ((!typeValue.IsArray) || (value.Count() == 1 && !fieldExpression.Type.Equals(typeof(DateTime))))
                    {
						string firstValue = value.FirstOrDefault();
						bool boolValue;
                        Expression<Func<T,bool>> lambda;
						if (bool.TryParse(firstValue, out boolValue))
						{
							var valueItem = Expression.Constant(boolValue, typeof(bool));

							var containsExpression = Expression.Equal(fieldExpression, valueItem);
							lambda = Expression.Lambda<Func<T, bool>>(containsExpression, param);
						}
						else
						{
							var valueItem = Convert.ChangeType(value.FirstOrDefault(), fieldExpression.Type);
							var containsMethod = typeof(string).GetMethod("Contains", new[] { fieldExpression.Type });

							var containsExpression = Expression.Call(fieldExpression, containsMethod, Expression.Constant(valueItem));
							lambda = Expression.Lambda<Func<T, bool>>(containsExpression, param);
						}
						

                        
                        source = source.Where(lambda);
                    }
                    else
                    {
                        var methods = typeof(Enumerable).GetMethods();
                        //var value = pair.Value as string[];
                        var constantExpressions = value.Select(x => fieldExpression.Type.Equals(typeof(DateTime)) ? Expression.Constant(DateTime.ParseExact(x, "yyyy-MM-dd", CultureInfo.InvariantCulture)):Expression.Constant(x));
                        var arrayExpression = Expression.NewArrayInit(fieldExpression.Type.Equals(typeof(DateTime)) ? typeof(DateTime) : typeof(string), constantExpressions);
                        var containsMethod = typeof(Enumerable).GetMethods()
                            .Where(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                            .Single()
                            .MakeGenericMethod(fieldExpression.Type.Equals(typeof(DateTime)) ? typeof(DateTime) : typeof(string));
                        Expression<Func<T, bool>> lambda;
                        if (fieldExpression.Type.Equals(typeof(DateTime)))
                        {
                            var formatValue = Convert.ChangeType(value.FirstOrDefault(), fieldExpression.Type);
                            var greaterThanOrEqual = Expression.GreaterThanOrEqual(fieldExpression, Expression.Constant(formatValue, fieldExpression.Type));
                            lambda = Expression.Lambda<Func<T, bool>>(greaterThanOrEqual, param);
                        }
                        else
                        {
                            var equality = Expression.Call(containsMethod, arrayExpression, fieldExpression);
                            lambda = Expression.Lambda<Func<T, bool>>(equality, param);
                        }
                        
                        source = source.Where(lambda);
                    }
                }
            }

            return source.AsQueryable();
        }
        public static IQueryable<T> ApplyTermFilter<T>(this IQueryable<T> source,
    IEnumerable<KeyValuePair<string,object>> values,
	Func<ParameterExpression, KeyValuePair<string, object>, Expression> fieldFunc)
        {
            var param = Expression.Parameter(typeof(T));

            foreach (var filter in values)
            {
				
				//var fieldExpression = GetFieldExpression(param, filter);
				var fieldExpression = fieldFunc(param, filter);
				if (fieldExpression != null)
                {
					var typeValue = filter.Value.GetType();
					var value = filter.Value as string[];
					var constantExpressions = value.Select(x => Expression.Constant(x));
                    var arrayExpression = Expression.NewArrayInit(typeof(string), constantExpressions);
                    var containsMethod = typeof(Enumerable).GetMethods()
                        .Where(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                        .Single()
                        .MakeGenericMethod(typeof(string));
                    var equality = Expression.Not(Expression.Call(containsMethod, arrayExpression, fieldExpression));
                    var lambda = Expression.Lambda<Func<T, bool>>(equality, param);
                    source = source.Where(lambda);
                }
            }

            return source;
        }
        public static Expression GetFieldExpression(ParameterExpression param, string fieldName)
        {
            var propertyInfo = typeof(T).GetProperty(fieldName);
            if (propertyInfo != null)
            {
                var memberExpression = Expression.Property(param, propertyInfo);
                return memberExpression;
            }
            else
            {
                throw new ArgumentException($"Field '{fieldName}' does not exist on type '{typeof(T).Name}'.");
            }
        }
		public static IQueryable<T> ApplyTermFilter<T>(this IQueryable<T> source,
		   IEnumerable<KeyValuePair<string, object>> values,
		   FieldSource fieldSource = FieldSource.Propety)
		{
			return ApplyTermFilter(source, values, GetFieldFunc(fieldSource));
		}
		public static IQueryable<T> FilterByValues<T>(this IQueryable<T> source,
           IEnumerable<KeyValuePair<string, object>> values,
           FieldSource fieldSource = FieldSource.Propety)
        {
            return FilterByValues(source, values, GetFieldFunc(fieldSource));
        }
        public static IQueryable<T> DynamicFilterByValues<T>(this IQueryable<T> source,
          IEnumerable<KeyValuePair<string, object>> values,
          FieldSource fieldSource = FieldSource.Propety)
        {
            return DynamicFilterByValues(source, values, GetFieldFunc(fieldSource));
        }
        public static IUpdatable<T> SetValues<T>(this IUpdatable<T> source,
           IEnumerable<KeyValuePair<string, object>> values,
           Func<ParameterExpression, KeyValuePair<string, object>, Expression> fieldFunc)
        {
            var param = Expression.Parameter(typeof(T));
            object current = source;
            foreach (var pair in values)
            {
                var fieldExpression = fieldFunc(param, pair);
                if (fieldExpression != null)
                {
                    var lambda = Expression.Lambda(fieldExpression, param);

                    var method = SetMethodInfo.MakeGenericMethod(typeof(T), fieldExpression.Type);
                    current = method.Invoke(null, new[] { current, lambda, pair.Value });
                }
            }

            return (IUpdatable<T>)current;
        }

        public static IUpdatable<T> SetValues<T>(this IQueryable<T> source,
           IEnumerable<KeyValuePair<string, object>> values,
           FieldSource fieldSource = FieldSource.Propety)
        {
            return source.AsUpdatable().SetValues(values, fieldSource);
        }

        public static IUpdatable<T> SetValues<T>(this IUpdatable<T> source,
           IEnumerable<KeyValuePair<string, object>> values,
           FieldSource fieldSource = FieldSource.Propety)
        {
            return SetValues(source, values, GetFieldFunc(fieldSource));
        }

        public static int UpdateDynamic<T>(this IQueryable<T> source,
           IEnumerable<KeyValuePair<string, object>> filterValues,
           IEnumerable<KeyValuePair<string, object>> setValues,
           FieldSource fieldSource = FieldSource.Propety)
        {
            return source
               .FilterByValues(filterValues, fieldSource)
               .SetValues(setValues, fieldSource)
               .Update();
        }

        public static Expression GetPropertyExpression(ParameterExpression instance, KeyValuePair<string, object> pair)
        {
            var propInfo = instance.Type.GetProperty(pair.Key);
            if (propInfo == null)
                return null;

            var propExpression = Expression.MakeMemberAccess(instance, propInfo);

            return propExpression;
        }

        public static Expression GetColumnExpression(ParameterExpression instance, KeyValuePair<string, object> pair)
        {
            var valueType = pair.Value != null ? pair.Value.GetType() : typeof(string);

            var method = SqlPropertyMethodInfo.MakeGenericMethod(valueType);
            var sqlPropertyCall = Expression.Call(null, method, instance, Expression.Constant(pair.Key, typeof(string)));
            var memberInfo = MemberHelper.GetMemberInfo(sqlPropertyCall);
            var memberAccess = Expression.MakeMemberAccess(instance, memberInfo);

            return memberAccess;
        }
    }
}
