﻿using System.Collections.ObjectModel;

namespace BlazorApp.Share.Enums;

public static class EnumerableExtension
{
    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size)
    {
        T[] array = null;
        var count = 0;
        foreach (var item in source)
        {
            if (array == null) array = new T[size];
            array[count] = item;
            count++;
            if (count == size)
            {
                yield return new ReadOnlyCollection<T>(array);
                array = null;
                count = 0;
            }
        }

        if (array != null)
        {
            Array.Resize(ref array, count);
            yield return new ReadOnlyCollection<T>(array);
        }
    }
}