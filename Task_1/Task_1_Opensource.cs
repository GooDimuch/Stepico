// execute online : todo

using System;
using System.Collections.Generic;
using Test_Stepico;

namespace Opensource {
	public class Test {
		/* ********************* */
		/* *** task function *** */

		// 1. - take out the initialization of racersNeedingRemoved, to prevent its regular creation in update
		private static List<Racer> racersNeedingRemoved = new List<Racer>(); // static modifier for example case only

		public static void UpdateRacers(float deltaTimeS, List<Racer> racers) {
			// 2. - move racersNeedingRemoved cleaning to the end

			// Updates the racers that are alive

			// 3. - count must be start from zero
			// 4. - the limit must be specified by the dimension of the list
			// 5. - no need to check racerIndex <= racers.Count

			// Updates the racers that are alive
			for (int racerIndex = 0; racerIndex < racers.Count; racerIndex++) {
				//Racer update takes milliseconds
				racers[racerIndex].Update(deltaTimeS * 1000.0f);
			}

			// 6. - do not check the same values
			// 7. - do not check already checked
			// 8. - with 20 entries in list, the old code will do 400 iterations, the new one - 190
			// Collides
			for (int racerIndex1 = 0; racerIndex1 < racers.Count - 1; racerIndex1++) {
				for (int racerIndex2 = racerIndex1 + 1; racerIndex2 < racers.Count; racerIndex2++) {
					Racer racer1 = racers[racerIndex1];
					Racer racer2 = racers[racerIndex2];

					if (racer1.IsCollidable() && racer2.IsCollidable() && racer1.CollidesWith(racer2)) {
						OnRacerExplodes(racer1);
						racersNeedingRemoved.Add(racer1);
						racersNeedingRemoved.Add(racer2);
					}
				}
			}

			// 9. - simplify and make cycles more correct
			for (int racerIndex = 0; racerIndex < racersNeedingRemoved.Count; racerIndex++) {
				racersNeedingRemoved[racerIndex].Destroy();
				racers.Remove(racersNeedingRemoved[racerIndex]);
			}

			// 10. - clear racersNeedingRemoved 
			racersNeedingRemoved.Clear();
		}
		/* *** task function *** */
		/* ********************* */


		public static void OnRacerExplodes(Racer racer) {
			Console.WriteLine("OnRacerExplodes with id = {0}", racer.Id);
		}

		/* entry point example */
		public static void Main(string[] args) {
			// init racers
			var racers = new List<Racer>();
			for (int id = 0; id < 20; ++id) {
				racers.Add(new Racer(id));
			}

			// initial state
			string debugStr = "Before = ";
			foreach (var racer in racers) {
				debugStr += string.Format("{0}; ", racer.Id);
			}

			Console.WriteLine(debugStr);

			// ! main logic !
			UpdateRacers(33, racers);

			// end state
			debugStr = "After = ";
			foreach (var racer in racers) {
				debugStr += string.Format("{0}; ", racer.Id);
			}

			Console.WriteLine(debugStr);
		}
		/* entry point example */
	}
}
