using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Test_Stepico {
	public class Task_1_OnlyRefactor {
		private const int SEC_TO_MILLISEC = 1000;
		private readonly List<Racer> _racersNeedingRemoved = new List<Racer>();
		private readonly List<Racer> _newRacerList = new List<Racer>();

		private void UpdateRacers(float deltaTimeS, List<Racer> racers) {
			_racersNeedingRemoved.Clear();

			// Updates the racers that are alive
			UpdateAliveRacers(deltaTimeS, racers);

			// Collides
			UpdateNeedRemovedList(racers);

			// Gets the racers that are still alive
			var newRacerList = GetNewRacerList(racers);

			// Get rid of all the exploded racers
			DestroyExplodedRacers(racers);

			// Builds the list of remaining racers
			UpdateRacerList(racers, newRacerList);

			ClearNewRacerList(newRacerList);
		}

		private void ClearNewRacerList(IList newRacerList) {
			newRacerList.Clear();
		}

		private void UpdateRacerList(List<Racer> racers, IEnumerable<Racer> newRacerList) {
			racers.Clear();
			racers.AddRange(newRacerList);
		}

		private void DestroyExplodedRacers(ICollection<Racer> racers) {
			foreach (var racer in _racersNeedingRemoved) {
				racer.Destroy();
				racers.Remove(racer);
			}
		}

		private List<Racer> GetNewRacerList(IEnumerable<Racer> racers) {
			_newRacerList.Clear();

			foreach (var racer in racers.Where(racer => !_racersNeedingRemoved.Contains(racer))) {
				_newRacerList.Add(racer);
			}

			return _newRacerList;
		}

		private void UpdateNeedRemovedList(List<Racer> racers) {
			foreach (var racer1 in racers.Where(racer => racer.IsCollidable())) {
				foreach (var racer2 in racers.Where(racer => racer.IsCollidable())) {
					if (racer1.Equals(racer2) || !racer1.CollidesWith(racer2)) continue;
					OnRacerExplodes(racer1);
					_racersNeedingRemoved.Add(racer1);
				}
			}
		}

		private void OnRacerExplodes(Racer racer) {
			throw new System.NotImplementedException();
		}

		private void UpdateAliveRacers(float deltaTimeS, IEnumerable<Racer> racers) {
			foreach (var racer in racers.Where(racer => racer.IsAlive())) {
				//Racer update takes milliseconds
				racer.Update(deltaTimeS * SEC_TO_MILLISEC);
			}
		}
	}
}