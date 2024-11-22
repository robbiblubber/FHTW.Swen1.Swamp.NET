using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using FHTW.Swen1.Swamp.Exceptions;

namespace FHTW.Swen1.Swamp
{
    /// <summary>This class implements a handler for user-specific requests.</summary>
    public class UserHandler: Handler, IHandler
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Handler                                                                                               //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Handles an incoming HTTP request.</summary>
        /// <param name="e">Event arguments.</param>
        public override bool Handle(HttpSvrEventArgs e)
        {
            if((e.Path.TrimEnd('/', ' ', '\t') == "/users") && (e.Method == "POST"))
            {
                JsonObject? reply = null;
                int status = HttpStatusCode.BAD_REQUEST;

                try
                {
                    JsonNode? json = JsonNode.Parse(e.Payload);
                    if(json != null)
                    {
                        User.Create((string) json["username"]!,
                                    (string) json["password"]!,
                                    (string?) json["fullname"] ?? "",
                                    (string?) json["email"] ?? "");
                        status = HttpStatusCode.OK;
                        reply = new JsonObject() { ["success"] = true,
                                                   ["message"] = "User created."};
                    }
                }
                catch(UserException ex)
                {
                    reply = new JsonObject() { ["success"] = false,
                                               ["message"] = ex.Message };
                }
                catch(Exception) 
                {
                    reply = new JsonObject() { ["success"] = false,
                                               ["message"] = "Invalid request." };
                }

                e.Reply(status, reply?.ToJsonString());
                return true;
            }
            else if((e.Path == "/users/me") && (e.Method == "GET"))
            {
                JsonObject? reply = null;
                int status = HttpStatusCode.BAD_REQUEST;

                (bool Success, User? User) ses = Token.Authenticate(e);

                if(ses.Success)
                {
                    status = HttpStatusCode.OK;
                    reply = new JsonObject() { ["success"] = true,
                                               ["username"] =  ses.User!.UserName,
                                               ["fullname"] =  ses.User!.FullName,
                                               ["email"] =  ses.User!.EMail };
                }
                else
                {
                    status = HttpStatusCode.UNAUTHORIZED;
                    reply = new JsonObject() { ["success"] = false,
                                               ["message"] = "Unauthorized." };
                }

                e.Reply(status, reply?.ToJsonString());
                return true;
            }
            else if(e.Path.StartsWith("user"))
            {
                // bla
                if(e.Method == "GET")
                {
                    // bla...
                }

                //e.Reply()
                return true;
            }

            return false;
        }
    }
}
