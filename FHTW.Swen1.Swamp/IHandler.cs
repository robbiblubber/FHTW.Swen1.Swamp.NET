using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>Request handlers implement this interface.</summary>
    public interface IHandler
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Checks if the handler can handle a request and handles the request if it does.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE if the event has been handled, otherwise returns FALSE.</returns>
        public bool Handle(HttpSvrEventArgs e);
    }
}
