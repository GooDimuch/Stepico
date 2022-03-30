using System.Collections.Generic;

namespace Test_Stepico.Task_3 {
	public class BackupService {
		public int Volume;

		public BackupService(int volume) {
			Volume = volume;
		}

		public List<DB> GetBestBackup(List<DB> dbList, out int price) {
			price = 0;

			//строим массив и закладываем место на ячейки пустышки 
			//выходящие из левого верхнего угла
			var cellInTable = new (int Price, List<DB> Databases)[dbList.Count + 1, Volume + 1];

			//проходим по всем вещам
			for (int i = 0; i < dbList.Count + 1; i++) {
				//проходим по всем рюкзакам
				for (int j = 0; j < Volume + 1; j++) {
					//попадаем в ячейку пустышку
					cellInTable[i, j].Databases = new List<DB>();
					
					if (i == 0 || j == 0) {
						cellInTable[i, j].Price = 0;
						cellInTable[i, j].Databases.Clear();
					}
					else {
						//если вес текущей вещи больше размера рюкзака
						//казалось бы откуда значение возьмется для первой вещи 
						//при таком условии. А оно возьмется из ряда пустышки
						if (dbList[i - 1].Volume > j) {
							cellInTable[i, j].Price = cellInTable[i - 1, j].Price;
							cellInTable[i, j].Databases.AddRange(cellInTable[i - 1, j].Databases);
						}
						else {
							//здесь по формуле. Значение над текущей ячейкой
							//Значение по вертикали: ряд вверх
							//и по горизонтали: вес рюкзака - вес текущей вещи
							var byFormula = dbList[i - 1].Price + cellInTable[i - 1, j - dbList[i - 1].Volume].Price;
							if (byFormula > cellInTable[i - 1, j].Price) {
								cellInTable[i, j].Price = byFormula;
								cellInTable[i, j].Databases.Add(dbList[i - 1]);
								cellInTable[i, j].Databases.AddRange(cellInTable[i - 1, j - dbList[i - 1].Volume].Databases);
							}
							else {
								cellInTable[i, j].Price = cellInTable[i - 1, j].Price;
								cellInTable[i, j].Databases.AddRange(cellInTable[i - 1, j].Databases);
							}
						}
					}
				}
			}

			// возвращаем правую нижнюю ячейку
			price = cellInTable[dbList.Count, Volume].Price;
			return cellInTable[dbList.Count, Volume].Databases;
		}
	}
}