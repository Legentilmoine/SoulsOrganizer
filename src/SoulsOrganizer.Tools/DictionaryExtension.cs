using System.Collections.Generic;

namespace SoulsOrganizer.Tools
{
    public static class DictionaryExtension
    {
        public static void AddOrReplace<K, V>(this Dictionary<K, V> dic, K key, V value)
        {
            if (dic.ContainsKey(key))
                dic[key] = value;
            else
                dic.Add(key, value);
        }
    }
   
}
