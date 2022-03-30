using System;

namespace Test_Stepico.Task_2 {
	public interface IJewelPosition {
		int X { get; }
		int Y { get; }
	}

	public struct JewelData : IJewelPosition, ICloneable {
		public enum JewelKindType {
			Empty,
			Red,
			Orange,
			Yellow,
			Green,
			Blue,
			// Indigo,
			Violet
		}

		public static JewelData Invalid { get; } = new JewelData(-1, -1);
		public static bool IsInvalid(JewelData data) => data.X == Invalid.X && data.Y == Invalid.Y;

		public int X { get; set; }
		public int Y { get; set; }
		public JewelKindType Type;

		public JewelData(int x, int y, JewelKindType type = JewelKindType.Empty) {
			X = x;
			Y = y;
			Type = type;
		}

		public bool IsSame(JewelData jewelData) => !IsInvalid(jewelData) && jewelData.Type == Type;

		public override bool Equals(object obj) =>
			obj is JewelData jewelData && jewelData.X == X && jewelData.Y == Y && jewelData.Type == Type;

		public object Clone() => new JewelData(X, Y, Type);

		public override string ToString() {
			return $"({Y}, {X}) {Type}";
		}
	}
}