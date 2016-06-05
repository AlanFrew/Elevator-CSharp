namespace Elevator {
    /// <summary>
    /// A button that can be pressed
    /// </summary>
    /// <remarks>Could be up, down, or a floor number</remarks>
	abstract class Button {
		protected Controller _controller {get; set;}
		
		protected Button(Controller controller) {
			_controller = controller;
		}

		internal abstract void Press();
	}
}