using System;
using System.Collections.Generic;

namespace AByte.UKit.Utilities
{
    public static class NodesHelper
    {
        //获取节点下，满足条件的所有节点，并返回
        public static IEnumerable<T> GetNodesByCondtion<T>(IList<T> nodes, Func<T, IList<T>> GetChilds, Func<T, bool> predicate)
        {
            if (nodes == null)
                yield break;
            foreach (var item in nodes)
            {
                if (predicate(item))
                {
                    yield return item;
                }
                var subList = GetNodesByCondtion(GetChilds(item), GetChilds, predicate);
                foreach (var sub in subList)
                {
                    yield return sub;
                }

            }
        }

    }
}


