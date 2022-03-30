using System.Collections.Generic;

namespace Test_Stepico.Task_2 {
	public struct MoveResult {
		public Move Move;
		public int Score;
		public List<JewelData> MatchList;

		public MoveResult(Move move, int score, List<JewelData> matchList) {
			Move = move;
			Score = score;
			MatchList = matchList;
		}

		public override string ToString() => $"{Move}, {Score}";
	}
}