using System;
using System.Text.Json.Nodes;



namespace FHTW.Swen1.Swamp
{
    public class SessionHandler: Handler, IHandler
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Handler                                                                                               //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Handles an incoming HTTP request.</summary>
        /// <param name="e">Event arguments.</param>
        public override bool Handle(HttpSvrEventArgs e)
        {
            if((e.Path.TrimEnd('/', ' ', '\t') == "/sessions") && (e.Method == "POST"))
            {                                                                   // POST /sessions will create a new session
                return _CreateSession(e);
            }

            return false;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a session.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE.</returns>
        public static bool _CreateSession(HttpSvrEventArgs e)
        {
            // TODO: implement
            return false;
        }
    }
}
