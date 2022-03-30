using System.Collections.Generic;
using System.Linq;

namespace Test_Stepico {
	public class Task_1_Reworked {
		private const int SEC_TO_MILLISEC = 1000;
		private readonly List<Racer> _collidingRacers = new List<Racer>();

		private void UpdateRacers(float deltaTimeS, List<Racer> racers) {
			UpdateAliveRacers(deltaTimeS, racers);
			DestroyCollidingRacers(racers);
			// UpdateCollidingRacers(racers);
			// DestroyCollidingRacers(racers, _collidingRacers);
		}

		private void UpdateAliveRacers(float deltaTimeS, IEnumerable<Racer> racers) {
			foreach (var racer in racers.Where(racer => racer.IsAlive())) {
				//Racer update takes milliseconds
				racer.Update(deltaTimeS * SEC_TO_MILLISEC);
			}
		}

		private void UpdateCollidingRacers(IEnumerable<Racer> racers) {
			_collidingRacers.Clear();
			foreach (var racer in racers.Where(racer => racer.CollidesWithAnotherRacer())) {
				_collidingRacers.Add(racer);
			}
		}

		private void DestroyCollidingRacers(ICollection<Racer> racers, List<Racer> collidingRacers) {
			foreach (var racer in collidingRacers) {
				OnRacerExplodes(racer);
				racers.Remove(racer);
				racer.Destroy();
			}
		}
		
		private void DestroyCollidingRacers(IList<Racer> racers) {
			for (var i = 0; i < racers.Count; i++) {
				if (!racers[i].CollidesWithAnotherRacer()) continue;

				OnRacerExplodes(racers[i]);
				racers.RemoveAt(i);
				racers[i].Destroy();
				i--;
			}
		}

		private void OnRacerExplodes(Racer racer) {
			throw new System.NotImplementedException();
		}
	}
}