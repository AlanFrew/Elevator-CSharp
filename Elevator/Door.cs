using System;

namespace Elevator {
    /// <summary>
    /// The door of the elevator, which can open and close
    /// </summary>
	class Door {
        private Car Car;

        internal Door(Car car)
        {
            Car = car;
        }

		public void Open() {
            Logger.Output("Car " + Car.Designation + " door opening");
		}

		public void Close() {
            Logger.Output("Car " + Car.Designation + " door closing");
        }
	}
}