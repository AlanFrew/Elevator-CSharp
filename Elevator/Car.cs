using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace Elevator {
    /// <summary>
    /// The elevator box that moves through the shaft
    /// </summary>
	class Car {
        //The current location of the elevator, which may be between floors
		internal double CurrentFloor { get; set; }

        //The name or number of the elevator
		internal char Designation { get; set; }

		internal Shaft Shaft { get; set; }

		private Door Door {get; set;}

        //The floor that the elevator is immediately moving toward
		private Stop _nextStop;
		internal Stop NextStop {
			get {
				return _nextStop;
			}

			set {
				_nextStop = value;

                if (value != null)
                {
                    Logger.Output("Car " + Designation + " accepts request to service floor " + _nextStop.Floor);
                }
			}
		}

        internal List<Stop> Destinations = new List<Stop>();

        //Whether the elevator is moving (or will next move) up or down
		internal Direction CurrentDirection { get; set; }

		internal Car(double startingFloor, Shaft shaft, char designation) {
			CurrentFloor = startingFloor;

			CurrentDirection = Direction.Null;

			Shaft = shaft;

			Designation = designation;

            Door = new Door(this);
		}

        //Called when the elevator has arrived at a floor and stopped
		internal bool HasMoved(Stop newStop) {
            Logger.Output("Car " + Designation + " is arriving at floor " + CurrentFloor);

            //Clear the current floor from the list of destinations
            foreach (var stop in Destinations)
            {
                if (stop.Floor == CurrentFloor)
                {
                    Destinations.Remove(stop);

                    break;
                }
            }

			Door.Open();

            var requestedDestination = Shaft.Controller.CheckForButtonPress(this);

            Thread.Sleep(500);

            if (requestedDestination != -1)
            {
                Logger.Output("Car " + Designation + " passenger requests floor " + requestedDestination);

                Destinations.Add(Shaft.Stops.First(stop => stop.Floor == requestedDestination));
            }

            Thread.Sleep(500);

            Door.Close();

            //Find next stop, or signal free
            if (Destinations.Count == 0)
            {
                NextStop = null;

                CurrentDirection = Direction.Null;

                return true;
            }
            else
            {
                if (Shaft.Car.CurrentDirection == Direction.Up)
                {
                    var higherFloors = Shaft.Car.Destinations.Where(destination => destination.Floor > CurrentFloor);

                    if (higherFloors.Count() > 0)
                    {
                        NextStop = Shaft.Stops[higherFloors.Min(destination => destination.Floor)];
                    }
                    else
                    {
                        NextStop = Shaft.Stops[Shaft.Car.Destinations.Max(destination => destination.Floor)];
                    }
                }
                else
                {
                    var lowerFloors = Shaft.Car.Destinations.Where(destination => destination.Floor < CurrentFloor);

                    if (lowerFloors.Count() > 0)
                    {
                        NextStop = Shaft.Stops[lowerFloors.Max(destination => destination.Floor)];
                    }
                    else
                    {
                        NextStop = Shaft.Stops[Shaft.Car.Destinations.Min(destination => destination.Floor)];
                    }
                }
            }

            return false;
		}
	}
}