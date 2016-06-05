namespace Elevator {
    /// <summary>
    /// A floor at which a particular elevator can stop and exchange passengers
    /// </summary>
	class Stop {
		internal int Floor { get; set; }

		internal CallPanel CallPanel {get; set;}

		internal Stop(int floorNumber) {
			Floor = floorNumber;

			CallPanel = Building.Instance.GetCallPanelForFloor(floorNumber);
		}
	}
}