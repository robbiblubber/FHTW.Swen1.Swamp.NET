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
            JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
            int status = HttpStatusCode.BAD_REQUEST;                            // initialize response

            try
            {
                JsonNode? json = JsonNode.Parse(e.Payload);                     // parse request JSON
                if(json != null)
                {                                                               // call User.Logon()
                    (bool Success, string Token) result = User.Logon((string) json["username"]!, (string) json["password"]!);

                    if(result.Success)
                    {                                                           // logon was successful
                        status = HttpStatusCode.OK;
                        reply = new JsonObject() { ["success"] = true,
                                                    ["message"] = "User created.",
                                                    ["token"] = result.Token };
                    }
                    else
                    {                                                           // logon failed
                        status = HttpStatusCode.UNAUTHORIZED;
                        reply = new JsonObject() { ["success"] = false,
                                                    ["message"] = "Logon failed." };
                    }
                }
            }
            catch(Exception) 
            {                                                                   // unexpected exception
                reply = new JsonObject() { ["success"] = false, ["message"] = "Unexpected error." };
            }

            e.Reply(status, reply?.ToJsonString());                             // send reply
            return true;
        }
    }
}
