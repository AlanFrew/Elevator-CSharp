using System;

namespace Elevator
{
    /// <summary>
    /// Controller for where to send output and debug info, since the application may run as console or WinForms
    /// </summary>
    static class Logger
    {
        internal static void Output(string message)
        {
            Console.WriteLine(message);

            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}