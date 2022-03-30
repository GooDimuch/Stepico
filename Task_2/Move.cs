namespace Test_Stepico.Task_2 {
	public struct Move : IJewelPosition {
		public enum MoveDirection {
			Up,
			Right,
			Down,
			Left
		}
			
		public JewelData JewelData;
		public readonly MoveDirection Direction;
		public int X => JewelData.X;
		public int Y => JewelData.Y;

		public Move(JewelData jewelData, MoveDirection direction) {
			JewelData = jewelData;
			Direction = direction;
		}

		public Move(int x, int y, MoveDirection direction) : this(new JewelData(x, y), direction) { }

		public override string ToString() => $"({JewelData.Y}, {JewelData.X}): {Direction}";
	}
}