using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class ListExtensions
    {
        public static T GetRandom<T>(this IList<T> self)
        {
            var index = Random.Range(0, self.Count);

            var result = self[index];
            return result;
        }
    }
}