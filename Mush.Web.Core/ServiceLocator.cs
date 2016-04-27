using System;
using System.Collections.Generic;

namespace Mush.Web.Core
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> dependencyDictionary = new Dictionary<Type, object>();

        public static void Register<T>(T dependencyItem)
        {
            dependencyDictionary[typeof(T)] = dependencyItem;
        }

        public static T Get<T>()
        {
            return (T)dependencyDictionary[typeof(T)];
        }
    }
}