using System;
using System.Collections.Generic;

namespace Opensource {
	class Board {
		enum JewelKind : int {
			Empty,
			Red,
			Orange,
			Yellow,
			Green,
			Blue,
			Indigo,
			Violet
		};

		enum MoveDirection : int {
			Up,
			Down,
			Left,
			Right,
			None //<-I'm using this to indicate that there's no move
		};

		struct Move {
			public int x;
			public int y;
			public MoveDirection direction;

			public Move(int x, int y, MoveDirection direction) {
				this.x = x;
				this.y = y;
				this.direction = direction;
			}
		};

		struct MoveResult {
			public Move move;
			public int score;
			public List<JewelData> matchList;
			public JewelKind[] boardData; //debug purpose

			public MoveResult(Move move, int score, List<JewelData> matchList, JewelKind[] boardData) {
				this.move = move;
				this.score = score;
				this.matchList = matchList;
				this.boardData = boardData;
			}
		};

		struct JewelData {
			public int x;
			public int y;

			public JewelData(int x, int y) {
				this.x = x;
				this.y = y;
			}
		}

		int width;
		int height;

		int GetWidth() {
			return 10;
		}

		int GetHeight() {
			return 10;
		}

		//BoardData Main
		JewelKind[] boardData;

		//BoardData Backup we use this to revert to original set
		JewelKind[] boardBackupData;
		JewelKind[] jewelAllowed;

		void Start() {
			boardData = new JewelKind[width * height];
			reshuffle();
		}

		void reshuffle() {
			fillBoard();
			while (haveAnyMatches()) {
				fillBoard();
			}

			boardBackupData = (JewelKind[])boardData.Clone();

			CalculateBestMoveForBoard();
		}

		void fillBoard() { }

		bool haveAnyMatches() {
			//List<JewelKind> match = new List<JewelKind>();
			for (int i = 0; i < boardData.Length; i++) {
				int x = i % width;
				int y = i / width;
				//scan left
				if (findMatch(x, y, MoveDirection.Right).Count >= 3) {
					return true;
				}

				//scan up;
				if (findMatch(x, y, MoveDirection.Up).Count >= 3) {
					return true;
				}
			}

			return false;
		}

		List<JewelData> checkAllMatches() {
			List<JewelData> result = new List<JewelData>();
			for (int i = 0; i < boardData.Length; i++) {
				int x = i % width;
				int y = i / width;
				List<JewelData> resultRight = findMatch(x, y, MoveDirection.Right);
				if (resultRight.Count >= 3) {
					foreach (JewelData jewel in resultRight) {
						addUnique(jewel, result);
					}
				}

				/*List<JewelData> resultLeft = findMatch(x, y, MoveDirection.Left);
				if (resultLeft.Count >= 3)
				{
				    foreach (JewelData jewel in resultLeft)
				    {
				        addUnique(jewel, result);
				    }

				}*/
				List<JewelData> resultUp = findMatch(x, y, MoveDirection.Up);
				if (resultUp.Count >= 3) {
					foreach (JewelData jewel in resultUp) {
						addUnique(jewel, result);
					}
				}
				/*List<JewelData> resultDown = findMatch(x, y, MoveDirection.Down);
				if (resultDown.Count >= 3)
				{
				    foreach (JewelData jewel in resultDown)
				    {
				        addUnique(jewel, result);
				    }

				}*/
			}

			return result;
		}

		bool addUnique(JewelData value, List<JewelData> list) {
			foreach (JewelData data in list) {
				if (data.x == value.x && data.y == value.y) {
					return true;
				}
			}

			list.Add(value);
			return false;
		}

		/// <summary>
		/// find a single match in 1 direction
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		/// 
		List<JewelData> findMatch(Move move) {
			return findMatch(move.x, move.y, move.direction);
		}

		List<JewelData> findMatch(int x, int y, MoveDirection direction) {
			List<JewelData> match = new List<JewelData>();
			JewelKind current = GetJewel(x, y);
			int matchNum = 0;
			bool _findMatch = true;
			switch (direction) {
				case MoveDirection.Left:


					while (_findMatch) {
						int target = x - matchNum;
						matchNum++;
						if (target >= 0) {
							if (current == GetJewel(target, y)) {
								JewelData currentJewel = new JewelData(target, y);
								match.Add(currentJewel);
							}
							else {
								_findMatch = false;
							}
						}
						else {
							_findMatch = false;
						}
					}

					return match;
				case MoveDirection.Right:

					while (_findMatch) {
						int target = x + matchNum;
						matchNum++;

						if (target <= width - 1) {
							if (current == GetJewel(target, y)) {
								JewelData currentJewel = new JewelData(target, y);
								match.Add(currentJewel);
							}
							else {
								_findMatch = false;
							}
						}
						else {
							_findMatch = false;
						}
					}

					return match;
				case MoveDirection.Up:

					while (_findMatch) {
						int target = y + matchNum;
						matchNum++;
						if (target <= height - 1) {
							if (current == GetJewel(x, target)) {
								JewelData currentJewel = new JewelData(x, target);
								match.Add(currentJewel);
							}
							else {
								_findMatch = false;
							}
						}
						else {
							_findMatch = false;
						}
					}

					return match;
				case MoveDirection.Down:

					while (_findMatch) {
						int target = y - matchNum;
						matchNum++;
						if (target >= 0) {
							if (current == GetJewel(x, target)) {
								JewelData currentJewel = new JewelData(x, target);
								match.Add(currentJewel);
							}
							else {
								_findMatch = false;
							}
						}
						else {
							_findMatch = false;
						}
					}

					return match;
				default:
					return match;
			}
		}

		JewelKind GetJewel(int x, int y) {
			return boardData[y * width + x];
		}

		void SetJewel(int x, int y, JewelKind kind) {
			boardData[y * width + x] = kind;
		}

		bool swapJewel(Move move) {
			boardData = (JewelKind[])boardBackupData.Clone();

			JewelKind pieceA = GetJewel(move.x, move.y);
			JewelKind pieceB = JewelKind.Empty;
			switch (move.direction) {
				case MoveDirection.Right:
					if (move.x < width - 1) {
						pieceB = GetJewel(move.x + 1, move.y);
						SetJewel(move.x + 1, move.y, pieceA);
					}
					else {
						return false;
					}

					break;
				case MoveDirection.Left:
					if (move.x > 0) {
						pieceB = GetJewel(move.x - 1, move.y);
						SetJewel(move.x - 1, move.y, pieceA);
					}
					else {
						return false;
					}

					break;
				case MoveDirection.Up:
					if (move.y < height - 1) {
						pieceB = GetJewel(move.x, move.y + 1);
						SetJewel(move.x, move.y + 1, pieceA);
					}
					else {
						return false;
					}

					break;
				case MoveDirection.Down:
					if (move.y > 0) {
						pieceB = GetJewel(move.x, move.y - 1);
						SetJewel(move.x, move.y - 1, pieceA);
					}
					else {
						return false;
					}

					break;
			}

			SetJewel(move.x, move.y, pieceB);
			return true;
		}
		//Implement this function

		Move CalculateBestMoveForBoard() {
			MoveResult bestResult;
			List<MoveResult> results = new List<MoveResult>();
			List<Move> moveToCheck = new List<Move>();
			for (int i = 0; i < boardData.Length; i++) {
				int x = i % width;
				int y = i / width;


				if (x < width - 1) {
					moveToCheck.Add(new Move(x, y, MoveDirection.Right));

					//scan right
				}

				if (x > 0) {
					moveToCheck.Add(new Move(x, y, MoveDirection.Left));
				}

				if (y < height - 1) {
					moveToCheck.Add(new Move(x, y, MoveDirection.Up));
				}

				if (y > 0) {
					moveToCheck.Add(new Move(x, y, MoveDirection.Down));
				}
			}

			foreach (Move m in moveToCheck) {
				if (swapJewel(m)) {
					List<JewelData> result = checkAllMatches();
					if (result.Count >= 3) {
						MoveResult moveResult = new MoveResult(m, result.Count, result, boardData);
						results.Add(moveResult);
					}
				}
			}

			boardData = (JewelKind[])boardBackupData.Clone();
			if (results.Count > 0) {
				if (results.Count == 1) {
					bestResult = results[0];
					return results[0].move;
				}
				else {
					int bestScore = results[0].score;
					Move bestMove = results[0].move;
					bestResult = results[0];
					for (int i = 1; i < results.Count; i++) {
						if (results[i].score > bestScore) {
							bestResult = results[i];
							bestScore = results[i].score;
							bestMove = results[i].move;
						}
					}

					return bestMove;
				}
			}
			else {
				return new Move(0, 0, MoveDirection.None);
			}
		}
	};
}