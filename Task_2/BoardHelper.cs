using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using MoveDirection = Test_Stepico.Task_2.Move.MoveDirection;

namespace Test_Stepico.Task_2 {
	public class BoardHelper {
		private const int MATCHES_FOR_SUCCESSFUL_MOVE = 3;

		private Board _board;
		private readonly List<MoveResult> _moveResults = new List<MoveResult>();
		private readonly List<Move> _movesToCheck = new List<Move>();

		private List<Move> MovesToCheck {
			get {
				if (_movesToCheck.Count == 0) UpdateAllMoves(_board.Matrix);
				return _movesToCheck;
			}
		}

		public BoardHelper(Board board) {
			_board = board;
		}

		public void UpdateBoard(Board board) {
			_board = board;
			_movesToCheck.Clear();
		}

		private void UpdateAllMoves(JewelData[,] boardMatrix) {
			boardMatrix.ForEach((y, x, _) => {
				_movesToCheck.AddRange(Enum.GetValues(typeof(MoveDirection))
					.Cast<MoveDirection>()
					.Select(moveDirection => new Move(boardMatrix[y, x], moveDirection)));
			});
		}

		public Move CalculateBestMoveForBoard(out MoveResult bestResult) =>
			GetBestMoveResult(GetAllMoveResults(), out bestResult);

		private static Move GetBestMoveResult(ICollection<MoveResult> results, out MoveResult bestResult) {
			if (results.Count > 0) {
				bestResult = results.MaxBy(result => result.Score);
				return bestResult.Move;
			}

			bestResult = default;
			return default;
		}

		private List<MoveResult> GetAllMoveResults() {
			_moveResults.Clear();

			foreach (var move in MovesToCheck.Where(move => IsValidMove(move, _board))) {
				// Console.WriteLine();
				// Console.WriteLine($"Move = {move}");
				var current = _board.GetJewel(move);
				var next = GetNext(move);
				if (current.IsSame(next)) {
					// Console.WriteLine(_board);
					// Console.WriteLine($"Current: {current}");
					// Console.WriteLine($"Next: {next}");
					// Console.WriteLine("current == next");
					continue;
				}

				MakeMove(move);

				// Console.WriteLine(_board);
				// Console.WriteLine($"Current: {current}");
				// Console.WriteLine($"Next: {next}");

				if (IsSuccessfulMove(move, _board, out var currentMatches)) {
					if (IsSuccessfulMove(GetNext(move), _board, out var nextMatches)) {
						currentMatches.AddRange(nextMatches);
					}

					_moveResults.Add(new MoveResult(move, currentMatches.Count, currentMatches));
				}

				UndoMove(move);
			}

			return _moveResults;
		}

		private void MakeMove(Move move) => _board.SwapJewels(move);
		private void UndoMove(Move move) => _board.SwapJewels(move);

		private static bool IsSuccessfulMove(IJewelPosition position, Board board, out List<JewelData> matchJewelList) {
			var matchesByVertical = GetMatchesByDirection(position, MoveDirection.Up, board);
			// Console.WriteLine("Vertical: " +
			//                   string.Join(", ", matchesByVertical.Select(data => data.ToString()).ToArray()));
			var matchesByHorizontal = GetMatchesByDirection(position, MoveDirection.Left, board);
			// Console.WriteLine("Horizontal: " +
			//                   string.Join(", ", matchesByHorizontal.Select(data => data.ToString()).ToArray()));

			var isSuccessfulVertical = IsSuccessfulMatches(matchesByVertical);
			var isSuccessfulHorizontal = IsSuccessfulMatches(matchesByHorizontal);

			// Console.WriteLine($"Vertical: {isSuccessfulVertical}");
			// Console.WriteLine($"Horizontal: {isSuccessfulHorizontal}");

			if (isSuccessfulVertical ||
			    isSuccessfulHorizontal) {
				var currentMatches = GetAllMatches(position, board, matchesByVertical, matchesByHorizontal);
				matchJewelList = new List<JewelData>();
				matchJewelList.AddRange(currentMatches);
				// Console.WriteLine("Matches: " +
				//                   string.Join(", ", matchJewelList.Select(data => data.ToString()).ToArray()));
				return true;
			}

			matchJewelList = null;
			return false;
		}

		private static bool IsSuccessfulMatches(ICollection matches) => matches.Count >= MATCHES_FOR_SUCCESSFUL_MOVE;

		private static List<JewelData>
			GetMatchesByDirection(IJewelPosition position, MoveDirection direction, Board board) {
			var matchesInDirection = new List<JewelData>() { board.GetJewel(position) };
			var findMatchByDirection = FindMatchByDirection(position, direction, board);
			// Console.WriteLine($"findMatchByDirection [{direction}]: " +
			//                   string.Join(", ", findMatchByDirection.Select(data => data.ToString()).ToArray()));

			matchesInDirection.AddRange(findMatchByDirection);
			var findMatchByReversDirection = FindMatchByDirection(position, GetReverseDirection(direction), board);
			// Console.WriteLine($"findMatchByReversDirection [{GetReverseDirection(direction)}]:" +
			//                   string.Join(", ", findMatchByReversDirection.Select(data => data.ToString()).ToArray()));

			matchesInDirection.AddRange(findMatchByReversDirection);
			return matchesInDirection;
		}

		private static List<JewelData> GetAllMatches(IJewelPosition position, Board board,
			List<JewelData> matchesVertical = null, List<JewelData> matchesHorizontal = null) {
			var currentJewel = board.GetJewel(position);
			var visited = new Dictionary<int, JewelData>() { { board.GetId(currentJewel), currentJewel } };
			AddRangeVisitedJewels(matchesVertical, visited, board);
			AddRangeVisitedJewels(matchesHorizontal, visited, board);
			var toBeVisited = visited.Clone();
			// Console.WriteLine($"toBeVisited[{toBeVisited.Count}]:" +
			//                   string.Join(", ", toBeVisited.Values.Select(data => data.ToString()).ToArray()));

			while (toBeVisited.Count > 0) {
				var notVisitedJewel = toBeVisited.First().Value;
				var notVisitedJewelId = board.GetId(notVisitedJewel);

				if (!currentJewel.IsSame(notVisitedJewel)) {
					toBeVisited.Remove(notVisitedJewelId);
					continue;
				}

				foreach (MoveDirection direction in Enum.GetValues(typeof(MoveDirection))) {
					var neighborJewel = GetNext(notVisitedJewel, direction, board);
					var neighborJewelId = board.GetId(neighborJewel);
					if (!visited.ContainsKey(neighborJewelId) &&
					    !toBeVisited.ContainsKey(neighborJewelId)) {
						toBeVisited.Add(neighborJewelId, neighborJewel);
					}
				}

				if (!visited.ContainsKey(notVisitedJewelId)) {
					visited.Add(notVisitedJewelId, notVisitedJewel);
				}

				toBeVisited.Remove(notVisitedJewelId);
			}

			// Console.WriteLine($"visited[{visited.Count}]:" +
			//                   string.Join(", ", visited.Values.Select(data => data.ToString()).ToArray()));


			return visited.Values.ToList();
		}

		private static void AddRangeVisitedJewels(List<JewelData> matchesVertical, IDictionary<int, JewelData> visited,
			Board board) {
			matchesVertical?.ForEach(data => {
				if (!visited.ContainsKey(board.GetId(data))) {
					visited.Add(board.GetId(data), data);
				}
			});
		}

		private static List<JewelData> FindMatchByDirection(IJewelPosition position, MoveDirection direction, Board board) {
			var matches = new List<JewelData>();
			var current = board.GetJewel(position);
			var next = GetNext(current, direction, board);

			while (current.IsSame(next)) {
				matches.Add(next);
				next = GetNext(next, direction, board);
			}

			return matches;
		}

		private static JewelData GetJewelUp(IJewelPosition position, Board board) =>
			IsValidPosition(position.X, position.Y - 1, board)
				? board.GetJewel(position.X, position.Y - 1)
				: JewelData.Invalid;

		private static JewelData GetJewelDown(IJewelPosition position, Board board) =>
			IsValidPosition(position.X, position.Y + 1, board)
				? board.GetJewel(position.X, position.Y + 1)
				: JewelData.Invalid;

		private static JewelData GetJewelLeft(IJewelPosition position, Board board) =>
			IsValidPosition(position.X - 1, position.Y, board)
				? board.GetJewel(position.X - 1, position.Y)
				: JewelData.Invalid;

		private static JewelData GetJewelRight(IJewelPosition position, Board board) =>
			IsValidPosition(position.X + 1, position.Y, board)
				? board.GetJewel(position.X + 1, position.Y)
				: JewelData.Invalid;


		private static bool IsValidPosition(int x, int y, Board board) =>
			x >= 0 && y >= 0 && x < board.Width && y < board.Height;

		private static bool IsValidMove(Move move, Board board) => IsValidMove(move, move.Direction, board);

		private static bool IsValidMove(IJewelPosition position, MoveDirection direction, Board board) {
			return direction switch {
				MoveDirection.Up => position.Y > 0 && GetJewelUp(position, board).Type != JewelData.JewelKindType.Empty,
				MoveDirection.Down => position.Y < board.Height - 1 &&
				                      GetJewelDown(position, board).Type != JewelData.JewelKindType.Empty,
				MoveDirection.Left => position.X > 0 && GetJewelLeft(position, board).Type != JewelData.JewelKindType.Empty,
				MoveDirection.Right => position.X < board.Width - 1 &&
				                       GetJewelRight(position, board).Type != JewelData.JewelKindType.Empty,
				_ => false
			};
		}

		public JewelData GetNext(Move move) => GetNext(move, move.Direction, _board);

		private static JewelData GetNext(IJewelPosition position, MoveDirection direction, Board board) {
			return direction switch {
				MoveDirection.Up => GetJewelUp(position, board),
				MoveDirection.Down => GetJewelDown(position, board),
				MoveDirection.Left => GetJewelLeft(position, board),
				MoveDirection.Right => GetJewelRight(position, board),
				_ => JewelData.Invalid
			};
		}

		private static MoveDirection GetReverseDirection(MoveDirection direction) {
			return direction switch {
				MoveDirection.Up => MoveDirection.Down,
				MoveDirection.Down => MoveDirection.Up,
				MoveDirection.Left => MoveDirection.Right,
				MoveDirection.Right => MoveDirection.Left,
				_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
			};
		}
	}
}