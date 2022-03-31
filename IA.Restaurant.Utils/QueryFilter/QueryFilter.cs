using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace IA.Restaurant.Utils.QueryFilter
{
    public static class FilterExpressionBuilder
    {
        public static List<FilterExpression>? BuildFilterExpression(this String queryfilter)
        {
            List<FilterExpression>? filtros = null;
            if (queryfilter != null)
                if (queryfilter.Length > 0)
                    filtros = JsonConvert.DeserializeObject<List<FilterExpression>>(queryfilter);

            return filtros;
        }
    }
    /// <summary>
    /// Class to build lambda expressions from filters
    /// </summary>
    public static class ExpressionBuilder
    {
        /// <summary>
        /// build lambda expressions from a list of filters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters">filter to apply</param>
        /// <returns>Expression Lambda</returns>
        public static Expression<Func<T, bool>>? ConstruyendoExpresion<T>(List<FilterExpression> filters)
        {
            if (filters == null)
                return null;
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression? exp = null;

            if (filters.Count == 1)
            {
                exp = RecuperaExpresion.GetExpression<T>(param, filters[0]);
            }
            else
            {
                exp = RecuperaExpresion.GetExpression<T>(param, filters[0]);

                for (int i = 1; i < filters.Count; i++)
                {

                    if (filters[i - 1].Conjunction != Conjunction.And)
                        exp = Expression.Or(exp, RecuperaExpresion.GetExpression<T>(param, filters[i]));
                    else
                        exp = Expression.AndAlso(exp, RecuperaExpresion.GetExpression<T>(param, filters[i]));
                }
            }
            return Expression.Lambda<Func<T, bool>>(exp, param);
        }
    }
    /// <summary>
    /// Static class to create lambda expressions from filters
    /// </summary>
    public static class RecuperaExpresion
    {

        private static MethodInfo? containsMethod = typeof(string).GetMethod("Contains",
            BindingFlags.Public | BindingFlags.Instance,
            null, CallingConventions.Any,
            new Type[] { typeof(String) }, null); //typeof(string).GetMethod("Contains");
        private static readonly MethodInfo? toStringMethod = typeof(object).GetMethod("ToString");
        private static readonly MethodInfo? Contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static readonly MethodInfo? startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static readonly MethodInfo? endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
        private static readonly MethodInfo? miTrim = typeof(string).GetMethod("Trim", Type.EmptyTypes);
        private static readonly MethodInfo? miLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

        /// <summary>
        /// Método para crear la expresión a aplicar por un filtro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">Alias de la expresión, EJEMPLO: t => </param>
        /// <param name="filter">Filtro a aplicar</param>
        /// <returns>Expresión</returns>
        public static Expression? GetExpression<T>(ParameterExpression param, FilterExpression filter)
        {
            var member = Expression.Property(param, filter.PropertyName);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType); // 1

            if (!converter.CanConvertFrom(typeof(string))) // 2
                throw new NotSupportedException();

            // Trata de convertir el valor al tipo de dato del campo enviado
            var propertyValue = filter.Value != null ? converter.ConvertFromInvariantString(filter.Value.ToString().Trim().ToLower()) : null; 
            var constant = Expression.Constant(propertyValue);
            var valueExpression = Expression.Convert(constant, propertyType); // 4

            switch (filter.Comparison)
            {
                case Comparison.Equal:
                    if (member.Type == typeof(int))
                    {
                        return Expression.Equal(member, constant);
                    }
                    else
                    {
                        return Expression.Equal(member, valueExpression);
                    }
                case Comparison.GreaterThan:
                    return Expression.GreaterThan(member, valueExpression);
                case Comparison.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, valueExpression);
                case Comparison.LessThan:
                    return Expression.LessThan(member, valueExpression);
                case Comparison.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, valueExpression);
                case Comparison.NotEqual:
                    return Expression.NotEqual(member, valueExpression);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Método para crear la expresión a aplicar en un subquery para busqueda en Contains
        /// </summary>
        /// <param name="param">Alias de la expresión, EJEMPLO: t =></param>
        /// <param name="properties">Son los campos en los que se realiza el filtrado.</param>
        /// <param name="searchText">Palabra a buscar</param>
        /// <returns>Expressión</returns>
        public static Expression? SearchPredicate(ParameterExpression param, IEnumerable<string> properties, string searchText)
        {
            Expression? exp = null;
            bool first = true;
            Expression? memberStringCall = null;
            Expression? trimMethod = null;
            Expression? toLowerMethod = null;
            UnaryExpression? valueExp = null;
            foreach (var valor in properties)
            {
                var typeProperty = Expression.Property(param, valor).Type;
                var constante = typeProperty == typeof(int) || typeProperty == typeof(int?) ? Expression.Constant(searchText.ToString().Trim().ToLower()) : Expression.Constant(searchText.ToString().Trim().ToLower());
                if (typeProperty == typeof(int) || typeProperty == typeof(int?))
                    memberStringCall = Expression.Call(Expression.Property(param, valor), toStringMethod);
                else
                {
                    var coalesce = Expression.Coalesce(Expression.PropertyOrField(param, valor), Expression.Constant(string.Empty));
                    valueExp = Expression.Convert(constante, ((PropertyInfo)Expression.Property(param, valor).Member).PropertyType);
                    trimMethod = Expression.Call(coalesce, miTrim);
                    toLowerMethod = Expression.Call(trimMethod, miLower);
                }

                if (first)
                {
                    exp = typeProperty == typeof(int) || typeProperty == typeof(int?) ? Expression.Call(memberStringCall, Contains, constante)
                        : Expression.Call(toLowerMethod, Contains, valueExp);
                    first = false;
                }
                else
                    exp = typeProperty == typeof(int) || typeProperty == typeof(int?) ? Expression.Or(exp, Expression.Call(memberStringCall, Contains, constante))
                        : Expression.Or(exp, Expression.Call(toLowerMethod, Contains, valueExp));
            }
            return exp;
        }

        /// <summary>
        /// Método para convertir una instancia a un queryable con el tipo de dato de la propiedad a aplicar el filtro
        /// </summary>
        /// <param name="instance">Instancia a convertir</param>
        /// <param name="typeConverter">Convertidor de la propiedad a aplicar el filtro</param>
        /// <param name="type">Tipo de dato de la propiedad de la entidad</param>
        /// <returns>IQueryable</returns>
        public static IQueryable ToQueryable(object instance, TypeConverter typeConverter, Type type)
        {
            // Se declara una lista, que funcione como cascaron para castear a IQueryable
            List<dynamic>? list = null;

            // Si el objeto es de tipo JArray 
            // (que se supone que debería ser así, ya que la propiedad Valor del filtro es un tipo object, y no lo castea a ningun tipo de dato)
            if (instance.GetType() == typeof(JArray))
            {
                // Convierte el objeto en JArray
                JArray jArray = JArray.FromObject(instance);
                // Lo convierte en una lista dinámica
                list = jArray.ToObject<List<dynamic>>();
            }
            // Si es un string "1,2,3" lo convierte a lista.
            else if (instance.GetType() == typeof(string))
            {
                list = instance?.ToString()?.Split(",").ToList<dynamic>();
            }
            // Si el objeto ya es una lista (cuando el filtro se hace desde el backend y no pasa por ninguna petición)
            else if (instance is System.Collections.IList)
            {
                System.Collections.IList? genericList = instance as System.Collections.IList;
                list = genericList?.Cast<object>().ToList();
            }
            // Si no, sólo hace una lista dinámica con el objeto
            else
                list = new List<dynamic>() { instance };

            // Crea una lista genérica con el tipo de dato de la propiedad
            var typedList = (System.Collections.IList?)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            // Por cada item en la lista, lo transforma en el tipo de dato que tiene la propiedad del In, 
            // para que el .Contains pueda validar el mismo tipo de dato
            var aux = list?.Select(i => typedList?.Add(typeConverter.ConvertFromInvariantString(i.ToString()))).ToList();
            // Retorna la lista como un queryable
            return typedList.AsQueryable();
        }
        /// <summary>
        /// Método de ordenación OrderBy
        /// </summary>
        /// <typeparam name="T">EL tipo</typeparam>
        /// <param name="source">Iqueryable</param>
        /// <param name="memberPath">Columnas a ordenar</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByMember<T>(this IQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "OrderBy");
        }
        /// <summary>
        /// Método de ordenación OrderByDescending
        /// </summary>
        /// <typeparam name="T">EL tipo</typeparam>
        /// <param name="source">Iqueryable</param>
        /// <param name="memberPath">Columnas a ordenar</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByMemberDescending<T>(this IQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "OrderByDescending");
        }
        /// <summary>
        /// Método de ordenación ThenBy
        /// </summary>
        /// <typeparam name="T">EL tipo</typeparam>
        /// <param name="source">Iqueryable</param>
        /// <param name="memberPath">Columnas a ordenar</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenByMember<T>(this IOrderedQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "ThenBy");
        }
        /// <summary>
        /// Método de ordenación ThenByDescending
        /// </summary>
        /// <typeparam name="T">EL tipo</typeparam>
        /// <param name="source">Iqueryable</param>
        /// <param name="memberPath">Columnas a ordenar</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenByMemberDescending<T>(this IOrderedQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "ThenByDescending");
        }
        /// <summary>
        /// Método para ordenar
        /// </summary>
        /// <typeparam name="T">EL tipo</typeparam>
        /// <param name="source">Iqueryable</param>
        /// <param name="memberPath">Columnas a ordenar</param>
        /// <param name="method">Dirección de ordenación</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> OrderByMemberUsing<T>(this IQueryable<T> source, string memberPath, string method)
        {
            var parameter = Expression.Parameter(typeof(T), "item");
            var member = memberPath.Split('.')
                .Aggregate((Expression)parameter, Expression.PropertyOrField);
            var keySelector = Expression.Lambda(member, parameter);
            var methodCall = Expression.Call(
                typeof(Queryable), method, new[] { parameter.Type, member.Type },
                source.Expression, Expression.Quote(keySelector));
            return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
        }
    }
    /// <summary>
    /// Class to apply filters to queries
    /// </summary>
    public class FilterExpression
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public Comparison Comparison { get; set; }
        public Conjunction Conjunction { get; set; }
    }

    /// <summary>
    /// Enumerable with the possible operations to apply in a filter
    /// </summary>
    public enum Comparison
    {
        Equal,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        NotEqual,
    }

    /// <summary>
    /// Enumerable with the logical Conjunction that apply between filters
    /// </summary>
    public enum Conjunction
    {
        And,
        Or
    }
}
