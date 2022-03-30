namespace Test_Stepico.Task_3 {
	public struct DB {
		public int Volume;
		public int Price;

		public DB(int volume, int price) {
			Volume = volume;
			Price = price;
		}

		public override string ToString() => $"[V: {Volume}, P: {Price}]";
	}
}