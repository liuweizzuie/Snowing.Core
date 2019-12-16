using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowing.Collections
{
    public static class IEnumableExtension
    {
        #region ConvertAll
        /// <summary>
        /// ConvertAll 将source中的每个元素转换为TResult类型
        /// </summary>       
        public static IEnumerable<TResult> ConvertAll<TObject, TResult>(this IEnumerable<TObject> source, Func<TObject, TResult> converter)
        {
            return source.ConvertWhere<TObject, TResult>(converter, null);
        }
        #endregion         

        #region ConvertWhere
        /// <summary>
        /// ConvertSpecification 将source中的符合predicate条件元素转换为TResult类型
        /// </summary>       
        public static IEnumerable<TResult> ConvertWhere<TObject, TResult>(this IEnumerable<TObject> source, Func<TObject, TResult> converter, Predicate<TObject> where)
        {
            IList<TResult> list = new List<TResult>();
            source.ActionWhere(delegate (TObject ele) { list.Add(converter(ele)); }, where);
            return list;
        }
        #endregion               

        #region ConvertFirst
        #region ConvertFirstSpecification
        /// <summary>
        /// ConvertSpecification 将source中的符合predicate条件的第一个元素转换为TResult类型
        /// </summary>       
        public static TResult ConvertFirst<TObject, TResult>(this IEnumerable<TObject> source, Func<TObject, TResult> converter, Predicate<TObject> predicate)
        {
            TObject target = source.First<TObject>(obj => predicate(obj));

            if (target == null)
            {
                return default(TResult);
            }

            return converter(target);
        }
        #endregion       
        #endregion

        #region ActionWhere
        /// <summary>
        /// ActionWhere 对集合中满足where条件的元素执行action。如果没有条件，predicate传入null。
        /// </summary>       
        public static void ActionWhere<TObject>(this IEnumerable<TObject> collection, Action<TObject> action, Predicate<TObject> where)
        {
            if (collection == null)
            {
                return;
            }

            if (where == null)
            {
                foreach (TObject obj in collection)
                {
                    action(obj);
                }

                return;
            }

            foreach (TObject obj in collection)
            {
                if (where(obj))
                {
                    action(obj);
                }
            }
        }
        #endregion

        #region ActionForeach
        /// <summary>
        /// ActionOnEach  对集合中的每个元素执行action。
        /// </summary>        
        public static void ActionForeach<TObject>(this IEnumerable<TObject> collection, Action<TObject> action)
        {
            collection.ActionWhere<TObject>(action, null);
        }
        #endregion
    }
}
