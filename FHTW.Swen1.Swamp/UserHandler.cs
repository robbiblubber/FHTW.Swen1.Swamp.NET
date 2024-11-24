using System;
using System.Text.Json.Nodes;

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
            {                                                                   // POST /users will create a user object
                return _CreateUser(e);
            }
            else if(e.Path.StartsWith("/users/") && (e.Method == "GET"))        // GET /users/UserName will query a user
            {
                return _QueryUser(e);
            }

            return false;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a user.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE.</returns>
        private static bool _CreateUser(HttpSvrEventArgs e)
        {
            JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
            int status = HttpStatusCode.BAD_REQUEST;                            // initialize response

            try
            {
                JsonNode? json = JsonNode.Parse(e.Payload);                     // parse payload
                if(json != null)
                {                                                               // submit payload to User.Create()
                    User.Create((string) json["username"]!,                     // will throw exception if failed.
                                (string) json["password"]!,
                                (string?) json["fullname"] ?? "",
                                (string?) json["email"] ?? "");
                    status = HttpStatusCode.OK;
                    reply = new JsonObject() { ["success"] = true,
                                                ["message"] = "User created."};
                }
            }
            catch(UserException ex)
            {                                                                   // handle UserException
                reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
            }
            catch(Exception) 
            {                                                                   // handle unexpected exception
                reply = new JsonObject() { ["success"] = false, ["message"] = "Unexpected error." };
            }

            e.Reply(status, reply?.ToJsonString());                             // send response
            return true;
        }


        /// <summary>Handles a user query request.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE.</returns>
        private static bool _QueryUser(HttpSvrEventArgs e)
        {
            JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
            int status = HttpStatusCode.BAD_REQUEST;                            // initialize response

            try
            {
                (bool Success, User? User) ses = Token.Authenticate(e);         // querying user information requires authentication

                if(ses.Success)
                {                                                               // authentication successful
                    User? user = User.Get(e.Path[7..]);                         // get requested user name

                    if(user == null)
                    {                                                           // user not found
                        status = HttpStatusCode.NOT_FOUND;
                        reply = new JsonObject() { ["success"] = false, ["message"] = "User not found." };
                    }
                    else
                    {
                        status = HttpStatusCode.OK;
                        reply = new JsonObject() { ["success"] = true,          // prepare response
                            ["username"] = user!.UserName,
                            ["fullname"] = user!.FullName,
                            ["email"] = user.EMail };
                    }
                }
                else
                {
                    status = HttpStatusCode.UNAUTHORIZED;
                    reply = new JsonObject() { ["success"] = false, ["message"] = "Unauthorized." };
                }
            }
            catch(Exception)
            {                                                                   // hanlde unexpected exception
                reply = new JsonObject() { ["success"] = false, ["message"] = "Unexpected error." };
            }

            e.Reply(status, reply?.ToJsonString());
            return true;
        }
    }
}
