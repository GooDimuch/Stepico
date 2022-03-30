using System;
using System.Linq;

namespace Test_Stepico.Task_2 {
	public class Board {
		public int Width { get; }
		public int Height { get; }
		public JewelData[,] Matrix { get; }

		private readonly Random _random = new Random(0);
		private readonly BoardHelper _boardHelper;

		private Board(int width, int height) {
			Width = width;
			Height = height;
			Matrix = new JewelData[width, height];
			_boardHelper = new BoardHelper(this);
		}

		public static Board Generate(int width) => Generate(width, width);

		public static Board Generate(int width, int height) {
			var board = new Board(width, height);
			board.RandomReset();
			return board;
		}

		private void RandomReset() =>
			Matrix.ForEach((y, x, jewelData) =>
				SetJewel(x, y, (JewelData.JewelKindType)_random.Next((int)JewelData.JewelKindType.Red, (int)JewelData.JewelKindType.Violet)));

		public int GetId(IJewelPosition position) => position.Y * Width + position.X;

		private void SetJewel(int x, int y, JewelData.JewelKindType type) {
			Matrix[y, x].X = x;
			Matrix[y, x].Y = y;
			Matrix[y, x].Type = type;
		}

		public JewelData GetJewel(IJewelPosition position) => GetJewel(position.X, position.Y);
		public JewelData GetJewel(int x, int y) => Matrix[y, x];

		public void SwapJewels(Move move) {
			SwapJewels(move, _boardHelper.GetNext(move));
		}

		public void SwapJewels(IJewelPosition position1, IJewelPosition position2) {
			var kind = GetJewel(position1);
			SetJewel(position1.X, position1.Y, GetJewel(position2).Type);
			SetJewel(position2.X, position2.Y, kind.Type);
		}

		//Implement this function
		public Move CalculateBestMoveForBoard(out MoveResult bestResult) {
			return _boardHelper.CalculateBestMoveForBoard(out bestResult);
		}

		public override string ToString() {
			var result = "";
			for (var i = 0; i < Matrix.GetLength(0); i++) {
				result += string.Join(",\t", Matrix.GetRow(i).Select(kind => kind.ToString()).ToArray()) + "\n";
			}

			return result;
		}
	}
}