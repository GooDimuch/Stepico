using System;
using System.Collections.Generic;
using System.Linq;

namespace Test_Stepico.Task_2 {
	public static class CustomArrayExtension {
		public static T[] GetColumn<T>(this T[,] matrix, int columnNumber) {
			return Enumerable.Range(0, matrix.GetLength(0))
				.Select(x => matrix[x, columnNumber])
				.ToArray();
		}

		public static T[] GetRow<T>(this T[,] matrix, int rowNumber) {
			return Enumerable.Range(0, matrix.GetLength(1))
				.Select(x => matrix[rowNumber, x])
				.ToArray();
		}

		public static void ForEach<T>(this T[,] matrix, Action<int, int, T> action) {
			for (var y = 0; y < matrix.GetLength(0); y++) {
				for (var x = 0; x < matrix.GetLength(1); x++) {
					action.Invoke(y, x, matrix[y, x]);
				}
			}
		}

		public static T[] ToArray<T>(this T[,] matrix) => matrix.Cast<T>().ToArray();

		public static List<T> ToList<T>(this T[,] matrix) => matrix.Cast<T>().ToList();

		public static Dictionary<TKey, TValue> Clone<TKey, TValue>
			(this Dictionary<TKey, TValue> original) where TValue : ICloneable {
			var ret = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
			foreach (var entry in original) {
				ret.Add(entry.Key, (TValue)entry.Value.Clone());
			}

			return ret;
		}
	}
}