using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Test_Stepico.Task_2 {
	public class Task_2 {
		private const int TABLE_SIZE = 10;

		public static void Main() {
			var board = Board.Generate(10);
			Console.WriteLine(board);
			Console.WriteLine($"{board.CalculateBestMoveForBoard(out var bestResult)} [{bestResult.Score}]\n" +
			                  string.Join("\n", bestResult.MatchList));

			// var times = new List<long>();
			// var stopwatch = new Stopwatch();
			// for (var i = 0; i < 100; i++) {
			// 	var board = Board.Generate(30);
			// 	stopwatch.Start();
			// 	board.CalculateBestMoveForBoard(out var bestResult);
			// 	stopwatch.Stop();
			// 	times.Add(stopwatch.ElapsedMilliseconds);
			// 	stopwatch.Reset();
			// }
			//
			// Console.WriteLine(string.Join(", ", times.Select(l => l.ToString()).ToArray()));
			// times.RemoveAt(0);
			// Console.WriteLine(times.Average());
		}
	}
}