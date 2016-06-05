using System.Collections.Generic;

namespace Elevator {
	/// <summary>
	/// A collection of one or more elevators that work together
	/// </summary>
	class Bank {
		internal List<Shaft> Shafts { get; set; }

		internal List<Car> Cars { get; set; }

		internal Controller Controller { get; set; }

		public Bank() {
			Controller = new Controller(this);
		}

		/// <summary>
		/// Constructs a bank of elevators
		/// </summary>
		/// <remarks>Assumes that each elevator can stop on each floor</remarks>
		public void Initialize(int shaftCount, int floorsPerShaft) {
			Cars = new List<Car>(shaftCount);
			Shafts = new List<Shaft>(shaftCount);
			
			var allFloors = new int[floorsPerShaft];

			for (int i = 0; i < floorsPerShaft; i++) {
				allFloors[i] = i;
			}

			for (int i = 0; i < shaftCount; i++) {
				var shaft = new Shaft(allFloors, (char)(i + 65), Controller);

				Shafts.Add(shaft);

				Cars.Add(shaft.Car);
			}
		}
	}
}