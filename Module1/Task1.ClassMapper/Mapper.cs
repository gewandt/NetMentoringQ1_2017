using System;

namespace Task1.ClassMapper
{
    public class Mapper<TSource, TDestination>
    {
        Func<TSource, TDestination> mapFunction;

        internal Mapper(Func<TSource, TDestination> func)
        {
            mapFunction = func;
        }

        public TDestination Map(TSource source)
        {
            return mapFunction(source);
        }
    }
}
