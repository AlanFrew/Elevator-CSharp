using System.Collections.Generic;

namespace Elevator {
    /// <summary>
    /// A structure containing one bank of elevators that service any number of floors, and any number of call panels
    /// </summary>
	public class Building {
		internal List<CallPanel> CallPanels { get; private set; }

		internal Bank Bank { get; private set; }

        #region singleton pattern
        public static Building Instance { get; private set; }

        public static void CreateInstance(int shafts, int floorsPerShaft) {
			Instance = new Building();

			Instance.Initialize(shafts, floorsPerShaft);
		}

		private Building() {
			//empty; prevent instantiation
		}
        #endregion

        private void Initialize(int shafts, int floorsPerShaft) {
			Bank = new Bank();

			CallPanels = new List<CallPanel>(floorsPerShaft);

			for (int i = 0; i < floorsPerShaft; i++) {
				CallPanels.Add(new CallPanel(i, Bank.Controller));
			}

			Bank.Initialize(shafts, floorsPerShaft);
		}

		internal CallPanel GetCallPanelForFloor(int floorNumber) {
			return CallPanels[floorNumber];
		}
	}
}