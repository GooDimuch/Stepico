using System;
using System.Collections.Generic;
using System.Linq;

namespace Test_Stepico.Task_3 {
	public class Task_3 {
		private const int DB_AMOUNT = 10;
		private const int BACKUP_VOLUME = 100;

		private static readonly Random Random = new Random(0);

		public static void Main() {
			var backupService = new BackupService(Random.Next(100, 1000));
			// var backupService = new BackupService(BACKUP_VOLUME);

			var dbList = new List<DB>(DB_AMOUNT);
			for (var i = 0; i < DB_AMOUNT; i++) {
				dbList.Add(new DB(Random.Next(0, 100), Random.Next(10, 20)));
			}

			var bestBackup = backupService.GetBestBackup(dbList, out var price) ?? new List<DB>();
			
			Console.WriteLine("==========Task=========");
			Console.WriteLine($"BackupVolume = {backupService.Volume}");
			Console.WriteLine("Databases:");
			Console.WriteLine(string.Join("\n", dbList.Select(db => db.ToString()).ToArray()));
			Console.WriteLine("==========Answer=========");
			Console.WriteLine($"{price}\n" + string.Join("\n", bestBackup.Select(db => db.ToString()).ToArray()));
		}
	}
}