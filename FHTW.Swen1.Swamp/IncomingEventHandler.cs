using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>Implements an event handler for Incoming HTTP events.</summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void IncomingEventHandler(object sender, HttpSvrEventArgs e);
}
