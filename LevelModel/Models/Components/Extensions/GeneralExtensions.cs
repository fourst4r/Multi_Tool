using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace LevelModel.Models.Components
{
    public static class GeneralExtensions
    {

        public static List<T> Merge<T>(this IEnumerable<T> list, IEnumerable<T> listToAdd) {
            list      = list      ?? new List<T>();
            listToAdd = listToAdd ?? new List<T>();

            return list.Concat(listToAdd).ToList();
        }

        public static void ForEach<T>(this IEnumerable<T> blocks, Action<T> action) {
            if (action == null)
                return;

            foreach (var b in blocks)
                action.Invoke(b);
        }

        public static string TrimEnd(this StringBuilder sb) {
            if (sb.Length != 0)
                sb.Length--;

            return sb.ToString();
        }

    }
}
