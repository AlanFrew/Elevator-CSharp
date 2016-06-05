namespace Elevator {
    /// <summary>
    /// A pair of buttons to summon the elevator
    /// </summary>
	internal class CallPanel {
        //The floor that the elevator will be summoned to
        internal int ContainingFloor { get; set; }

        internal CallButton UpButton { get; set; }

		internal CallButton DownButton { get; set; }

		internal Controller Controller { get; set; }

		internal CallPanel(int containingFloor, Controller controller) {
			ContainingFloor = containingFloor;
			Controller = controller;

			UpButton = new CallButton(this, Direction.Up, controller);
			DownButton = new CallButton(this, Direction.Down, controller);
		}

		internal void TurnOffLight(Direction direction) {
			if (direction == Direction.Down) {
				DownButton.IsLit = false;
			}
			else if (direction == Direction.Up) {
				UpButton.IsLit = false;
			}
		}
	}
}
