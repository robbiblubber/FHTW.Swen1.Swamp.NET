using System;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace FHTW.Swen1.Swamp
{
    /// <summary>This class implements a user handler.</summary>
    public class UserHandler: Handler, IHandler
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private methods                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a user.</summary>
        /// <param name="e"></param>
        private void _CreateUser(HttpSvrEventArgs e)
        {
            JsonNode? j = JsonNode.Parse(e.Payload);
            if(j != null) 
            {
                try
                {
                    User rval = User.Create((string?) j["id"] ?? "", (string?) j["name"] ?? "", (string?) j["password"] ?? "");
                    e.Reply(HttpCodes.OK, "{\"msg\":\"User created.\"}");
                }
                catch(Exception) 
                {
                    e.Reply(HttpCodes.BAD_REQUEST, "{\"msg\":\"User could not be created.\"}");
                }
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] IHandler                                                                                              //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Checks if the handler can handle a request and handles the request if it does.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE if the event has been handled, otherwise returns FALSE.</returns>
        public override bool Handle(HttpSvrEventArgs e)
        {
            if(e.Path.StartsWith("/users/"))
            {

            }
            else if(e.Path == "/users")
            {
                if(e.Method == "POST")
                {
                    _CreateUser(e);
                }
                else { e.Reply(400); }
            }
            else { return false; }

            return true;
        }
    }
}
