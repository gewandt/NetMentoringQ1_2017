using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Task1.ClassMapper
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));
            var destExp = Expression.New(typeof(TDestination));

            var sameProperties = GetSameProperties<TSource, TDestination>();
            var bindMembers = sameProperties
                .Select(c => Expression.Bind(destExp.Type.GetProperty(c.Name), Expression.Call(sourceParam, c.GetGetMethod())))
                .Cast<MemberBinding>()
                .ToList();

            var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(
                Expression.MemberInit(destExp, bindMembers), sourceParam);

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        private static IEnumerable<PropertyInfo> GetSameProperties<TSource, TDestination>()
        {
            var sourceProps = typeof(TSource).GetProperties();
            var destProps = typeof(TDestination).GetProperties();

            return (from sourceProp in sourceProps
                    from destProp in destProps
                    where destProp.Name == sourceProp.Name && 
                    destProp.PropertyType.FullName == sourceProp.PropertyType.FullName
                    select sourceProp);
        }
    }
}
