using System;
using System.Threading.Tasks;
using System.Threading;

namespace Elevator {
	static class Program {
        delegate void ButtonPressDelegate(CallRequest request);

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
            //This is a test program showcasing a building with 10 floors and two elevators
			Building.CreateInstance(2, 10);

			var testBuilding = Building.Instance;

            //start with some call buttons already pressed
			testBuilding.GetCallPanelForFloor(5).UpButton.Press();
            testBuilding.GetCallPanelForFloor(6).DownButton.Press();

            //queue some requests for passengers once they get into an elevator
            testBuilding.Bank.Controller.ButtonPresses.Add(7);
            testBuilding.Bank.Controller.ButtonPresses.Add(4);

            //prepare some callButton presses to be added in real time
            ButtonPressDelegate newPassenger1 = testBuilding.Bank.Controller.PressOutsideButton;

            var task = new Task(testBuilding.Bank.Controller.RunAll);
            task.Start();

            Thread.Sleep(50);

            //Simulate a button press on the fly
            newPassenger1(new CallRequest(3, Direction.Up));

			Console.ReadLine();
		}
	}
}