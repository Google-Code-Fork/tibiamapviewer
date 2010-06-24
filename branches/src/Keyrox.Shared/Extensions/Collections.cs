using System.Collections.Generic;
using System.Linq;

namespace Keyrox.Shared.Collections {
    public static class Collections {

        /// <summary>
        /// Gets the item.
        /// </summary>
        public static object GetItem<T>(this IList<T> list, T value) {
            var item = list.Where(i => i.GetHashCode() == value.GetHashCode());
            if(item.Count() > 0) {
                return item.First();
            }
            return null;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        public static object GetItem<T>(this IList<T> list, string value) {
            var item = list.Where(i => i.ToString() == value);
            if (item.Count() > 0) {
                return item.First();
            }
            return null;
        }

        /// <summary>
        /// Adds if not exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="items">The items.</param>
        public static void AddIfNotExist<T>(this IList<T> list, T[] items) where T : class {
            foreach (T item in items) {
                if (!list.Contains(item)) { list.Add(item); }
            }
        }

        /// <summary>
        /// Determines whether the specified list contains any of defined values.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static object ContainsAnyOf(this object[] list, object[] values){
            foreach (object obj in values) {
                if (list.Contains(obj)) return obj;
            }
            return null;
        }

        /// <summary>
        /// Gets the next.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="currentItem">The current item.</param>
        /// <returns></returns>
        public static T GetNext<T>(this List<T> list, T currentItem) {
            var nextIndex = list.IndexOf(currentItem) + 1;
            if (nextIndex >= list.Count) nextIndex = 0;
            return list[nextIndex];
        }
    }
}
