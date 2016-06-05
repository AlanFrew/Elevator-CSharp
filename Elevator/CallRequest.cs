namespace Elevator
{
    /// <summary>
    /// A request for an elevator from a particular floor, going in a particular direction
    /// </summary>
    class CallRequest
    {
        internal CallRequest(int floor, Direction direction)
        {
            Floor = floor;
            Direction = direction;
        }

        internal int Floor { get; set; }

        internal Direction Direction { get; set; }
    }
}