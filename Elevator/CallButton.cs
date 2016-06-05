using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Elevator {
    /// <summary>
    /// A button that summons that elevator
    /// </summary>
	internal class CallButton : Button {
        private CallPanel CallPanel;

        //Whether the button is lit, indicating that an elevator has been summoned
		internal bool IsLit { get; set; }

        //Whether the passenger wants to go up or down
		internal Direction Direction { get; set; }

		internal CallButton(CallPanel callPannel, Direction direction, Controller controller)
			: base(controller) {
            CallPanel = callPannel;
			Direction = direction;
			IsLit = false;
		}
		
		internal override void Press() {
			Logger.Output(Direction + " elevator called from floor " + CallPanel.ContainingFloor);

			_controller.RequestElevator(new CallRequest(CallPanel.ContainingFloor, Direction));

			IsLit = true;
		}
	}
}