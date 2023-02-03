using System.Collections.Generic;

namespace AByte.UKit.Extensions
{

    /// <summary>
    /// Dictionary 字典扩展 
    /// </summary>
    public static class DictionaryExtensions
    {
        public static Tvalue TryGet<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dic, Tkey key)
        {
            Tvalue value;
            dic.TryGetValue(key, out value);
            return value;
        }

        public static void TryAdd<Tkey, Titem>(this Dictionary<Tkey, List<Titem>> dic, Tkey key, Titem item)
        {
            if(dic == null)dic  = new Dictionary<Tkey, List<Titem>>();
            if (dic.ContainsKey(key))
                dic[key].Add(item);
            else
                dic.Add(key, new List<Titem> { item });
        }

        public static void TryAdd<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dic, Tkey key, Tvalue value)
        {

            if (dic == null) dic = new Dictionary<Tkey, Tvalue>();
            if (dic.ContainsKey(key))
                dic[key] = value;
            else
                dic.Add(key, value);
        }

    }
}

