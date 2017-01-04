using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevator {
    /// <summary>
    /// The orchestrator that tells elevators where to go, using one of various algorithms
    /// </summary>
	class Controller {
        //Simulation of people calling the elevator, for testing purposes
        internal List<int> ButtonPresses = new List<int>();

        private Bank Bank { get; set; }

		internal Controller(Bank bank) {
			Bank = bank;
		}

        //Floors where the call button has been pressed but currently have no elevator assigned
        internal List<CallRequest> UnservicedFloors = new List<CallRequest>();

        //Find an elevator to service a requested floor
        internal void RequestElevator(CallRequest request)
        {
            bool carFound = false;

            lock (UnservicedFloors)
            {
                foreach (var car in Bank.Cars)
                {
                    if (car.NextStop == null && car.Shaft.CanAccessFloor(request.Floor))
                    {
                        Logger.Output("Car " + car.Designation + " requested to service floor " + request.Floor);

                        car.NextStop = car.Shaft[request.Floor];

                        carFound = true;

                        break;
                    }
                }

                if (carFound == false)
                {
                    Logger.Output("Request to floor " + request.Floor + " cannot be serviced immediately");

                    UnservicedFloors.Add(request);
                }
            }
        }

        //See if a call button was pressed that the given elevator can service en route
        internal void CheckForNewStop(Car movingCar)
        {
            var requestsNewlyServiced = new List<CallRequest>();
            lock (UnservicedFloors)
            {
                foreach (var request in UnservicedFloors)
                {
                    //see if the request is on the way
                    var floor = request.Floor;
                    if ((movingCar.CurrentFloor < floor && floor < movingCar.NextStop.Floor && movingCar.CurrentDirection == request.Direction && request.Direction == Direction.Up) ||
                        (movingCar.CurrentFloor > floor && floor > movingCar.NextStop.Floor && movingCar.CurrentDirection == request.Direction && request.Direction == Direction.Down))
                    {
                        movingCar.Destinations.Add(movingCar.NextStop);

                        Logger.Output("Car " + movingCar.Designation + " adds floor " + request.Floor + " as an intermediate stop");

                        movingCar.NextStop = movingCar.Shaft.Stops.Single(stop => stop.Floor == request.Floor);

                        requestsNewlyServiced.Add(request);

                        
                    }
                }

                foreach (var request in requestsNewlyServiced)
                {
                    UnservicedFloors.Remove(request);
                }
            }
            //requestsNewlyServiced.ForEach(request => );
        }

        //Run the elevators once the test data is set up
        internal void RunAll()
        {
            foreach (var shaft in Bank.Shafts)
            {
                var task = new Task(shaft.Move);

                task.Start();
            }
        }

        //Alert the controller that an elevator is ready for new instructions
        internal void SignalFreeCar(Car car)
        {
            Logger.Output("Car " + car.Designation + " signals ready");

            if (UnservicedFloors.Count > 0)
            {
                car.NextStop = car.Shaft.Stops.Single(stop => stop.Floor == UnservicedFloors.First().Floor);

                UnservicedFloors.Remove(UnservicedFloors.First());

                car.Shaft.RequestMovement(car, car.NextStop);
            }
        }

        //Check if a passenger has entered the elevator and pressed a button
        internal int CheckForButtonPress(Car car)
        {
            if (ButtonPresses.Any())
            {
                var result = ButtonPresses.First();

                ButtonPresses.Remove(ButtonPresses.First());

                return result;
            }

            return -1;
        }

        //Simulate someone calling an elevator and attempt to service the request
        internal void PressOutsideButton(CallRequest request)
        {
            Logger.Output("Someone pressed the call button on floor " + request.Floor);

            RequestElevator(request);
        }
	}
}