using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
    private static readonly Random _random = new Random();
    private static readonly object _syncLock = new object();

    public static T RandomNonEmptyElement<T>(this IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        // 优化对 IList<T> 的处理（如 List<T>、数组等）
        var list = collection as IList<T> ?? collection.ToList();
        
        if (list.Count == 0)
            throw new InvalidOperationException("集合不能为空");
        
        int index;
        lock (_syncLock)
        {
            index = _random.Next(list.Count);
        }
        return list[index];
    }
}