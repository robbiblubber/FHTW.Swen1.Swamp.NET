using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using FHTW.Swen1.Swamp.Exceptions;

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
            {
                JsonObject? reply = null;
                int status = HttpStatusCode.BAD_REQUEST;

                try
                {
                    JsonNode? json = JsonNode.Parse(e.Payload);
                    if(json != null)
                    {
                        (bool Success, string Token) result = User.Logon((string) json["username"]!, (string) json["password"]!);

                        if(result.Success)
                        {
                            status = HttpStatusCode.OK;
                            reply = new JsonObject() { ["success"] = true,
                                                       ["message"] = "User created.",
                                                       ["token"] = result.Token };
                        }
                        else
                        {
                            status = HttpStatusCode.UNAUTHORIZED;
                            reply = new JsonObject() { ["success"] = false,
                                                       ["message"] = "Logon failed." };
                        }
                    }
                }
                catch(Exception) 
                {
                    reply = new JsonObject() { ["success"] = false,
                                               ["message"] = "Invalid request." };
                }

                e.Reply(status, reply?.ToJsonString());
                return true;
            }

            return false;
        }
    }
}
