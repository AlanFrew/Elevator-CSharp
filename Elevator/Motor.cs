using System.Threading;
using System;

namespace Elevator {
    /// <summary>
    /// A simulation of the motor which "actually" moves the elevator
    /// </summary>
	class Motor {
		Shaft Shaft {get; set;}

		internal Motor(Shaft shaft) {
			Shaft = shaft;
		}

		internal void Move() {
            var moveUnit = 0.2;
                    
            while (Shaft.Car.CurrentFloor != Shaft.Car.NextStop.Floor)
            {
                if (Shaft.Car.CurrentDirection == Direction.Up)
                {
                    Shaft.Car.CurrentFloor = Math.Round(Shaft.Car.CurrentFloor + moveUnit, 8);
                }
                else
                {
                    Shaft.Car.CurrentFloor = Math.Round(Shaft.Car.CurrentFloor - moveUnit, 8);
                }

                Thread.Sleep(50);		//simulate time taken to move

                Shaft.Controller.CheckForNewStop(Shaft.Car);               
            }

            Shaft.HasMoved();
        }
	}
}