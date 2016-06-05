using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Elevator {
    /// <summary>
    /// The column that the elevator moves through, containing variable floors that the elevator can stop at
    /// </summary>
	class Shaft {
		internal List<Stop> Stops { get; set; }

		internal Car Car { get; set; }

		private Motor Motor { get; set; }

        internal Controller Controller { get; set; }

		internal Shaft(int[] floorsWithStops, char designation, Controller controller) {
			Stops = new List<Stop>(floorsWithStops.Length);

			foreach (var floor in floorsWithStops) {
				Stops.Add(new Stop(floor));
			}

			Car = new Car(0, this, designation);

			Motor = new Motor(this);

            Controller = controller;
		}

        //Make the elevator move to a stop
		internal void RequestMovement(Car car, Stop destination) {
            if (destination.Floor > car.CurrentFloor)
            {
                car.CurrentDirection = Direction.Up;
            }
            else
            {
                car.CurrentDirection = Direction.Down;
            }

			var leavingFloor = car.CurrentFloor;

            Logger.Output("Car " + car.Designation + " is leaving floor " + leavingFloor);

            Motor.Move();        
		}

        //Cleanup activity, called when the elevator has arrived at its destination
		internal void HasMoved() {
            var newStop = Stops[(int)Car.CurrentFloor];

			newStop.CallPanel.TurnOffLight(Car.CurrentDirection);

			var carIsFree = Car.HasMoved(newStop);

            if (carIsFree)
            {
                Controller.SignalFreeCar(Car);
            }
            else
            {
                Move();
            }
		}

        //Currently all elevators access all floors
		internal bool CanAccessFloor(int floorNumber) {
			return Stops.Any(stop => stop.Floor == floorNumber);
		}

        //helper function
        internal void Move()
        {
            if (Car.NextStop != null)
            {
                RequestMovement(Car, Car.NextStop);
            }
        }

        //helper function--property indexer
        internal Stop this[int floor]{
			get {
				return Stops.FirstOrDefault(stop => stop.Floor == floor);
			}
		}
	}
}