using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyRestClient
{
    internal class Utility
    {
        public static IList<Type> FindTypesBy(Type type, Func<Type, bool> predicate = null)
        {
            var foundTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type.IsAssignableFrom);

            if (predicate != null)
            {
                foundTypes = foundTypes.Where(predicate);
            }

            return foundTypes.ToList();
        }

        public static IList<Type> FindImplementationClasses(Type type, bool includeAbstract = false)
        {
            return FindTypesBy(type, t => t.IsClass && t.IsAbstract == includeAbstract);
        }

        public static IList<Type> FindTypesBy<T>(Func<Type, bool> predicate = null) where T : class
        {
            return FindTypesBy(typeof(T), predicate);
        }

        public static IList<Type> FindImplementationClasses<T>(bool includeAbstract = false) where T : class
        {
            return FindImplementationClasses(typeof(T), includeAbstract);
        }
    }
}