using System;
using System.Security;
using System.Text.Json.Nodes;

using FHTW.Swen1.Swamp.Security;
using FHTW.Swen1.Swamp.Server;



namespace FHTW.Swen1.Swamp.Handlers
{
    /// <summary>This class implements a handler for user-specific requests.</summary>
    public sealed class UserHandler: Handler, IHandler
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Handler                                                                                               //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Handles an incoming HTTP request.</summary>
        /// <param name="e">Event arguments.</param>
        public override bool Handle(HttpSvrEventArgs e)
        {
            if (e.Path.TrimEnd('/', ' ', '\t') == "/users" && e.Method == "POST")
            {                                                                   // POST /users will create a user object
                return _CreateUser(e);
            }
            else if (e.Path.StartsWith("/users/") && e.Method == "GET")        // GET /users/UserName will query a user
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
                {
                    User user = new()                                           // create user object
                    {
                        UserName = (string?) json["username"] ?? "",
                        FullName = (string?) json["name"] ?? "",
                        EMail = (string?) json["email"] ?? ""
                    };
                    user.BeginEdit(Session.From(e));                            // edit user with session

                    user.IsAdmin = (bool?) json["admin"] ?? false;              // set admin flag                    
                    user.Save((string?) json["password"] ?? "12345");           // save user object
                    
                    user.EndEdit();                                             // end editing

                    status = HttpStatusCode.OK;
                    reply = new JsonObject() { ["success"] = true,
                                                ["message"] = "User created."};
                }
            }
            catch(SecurityException ex)
            {                                                                   // handle SecurityException
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
                Session ses = Session.From(e);

                if(ses.Valid)
                {                                                               // authentication successful
                    try
                    {
                        User? user = User.ByUserName(e.Path[7..]);              // get requested user

                        status = HttpStatusCode.OK;
                        reply = new JsonObject() { ["success"] = true,          // prepare response
                            ["username"] = user!.UserName,
                            ["fullname"] = user!.FullName,
                            ["email"] = user.EMail };
                    }
                    catch(Exception)
                    {                                                           // user not found
                        status = HttpStatusCode.NOT_FOUND;
                        reply = new JsonObject() { ["success"] = false, ["message"] = "User not found." };
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
