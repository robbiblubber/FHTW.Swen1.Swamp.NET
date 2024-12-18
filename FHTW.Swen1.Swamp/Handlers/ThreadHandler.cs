using System;
using System.Security;
using System.Text.Json.Nodes;

using Thread = FHTW.Swen1.Swamp.Model.Thread;

using FHTW.Swen1.Swamp.Security;
using FHTW.Swen1.Swamp.Server;
using FHTW.Swen1.Swamp.Model;



namespace FHTW.Swen1.Swamp.Handlers
{
    /// <summary>This class implements a handler for threads.</summary>
    public sealed class ThreadHandler: Handler, IHandler
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Handler                                                                                               //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Handles an incoming HTTP request.</summary>
        /// <param name="e">Event arguments.</param>
        public override bool Handle(HttpSvrEventArgs e)
        {
            if(e.Path.TrimEnd('/', ' ', '\t') == "/threads" && e.Method == "POST")
            {                                                                   // POST /threads will create a thread object
                return _Create(e);
            }
            else if(e.Path.StartsWith("/threads/"))
            {
                if(e.Method == "GET")
                {
                    if(e.Path.TrimEnd('/').EndsWith("entries"))
                    {
                        return _GetEntries(e);
                    }
                    else { return _Query(e); }
                }
                else if(e.Method == "PUT")
                {
                    return _Edit(e);
                }
                else if(e.Method == "DELETE") 
                {
                    return _Delete(e);
                }
            }

            return false;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a thread.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE.</returns>
        private static bool _Create(HttpSvrEventArgs e)
        {
            JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
            int status = HttpStatusCode.BAD_REQUEST;                            // initialize response

            try
            {
                JsonNode? json = JsonNode.Parse(e.Payload);                     // parse payload
                if(json != null)
                {
                    Thread th = new() { Title = (string?) json["title"] ?? string.Empty };
                    th.BeginEdit(Session.From(e));                              // edit thread with session

                    th.Save();                                                  // save thread
                    
                    th.EndEdit();                                               // end editing

                    status = HttpStatusCode.OK;
                    reply = new JsonObject() { ["success"] = true,
                                               ["id"] = th.ID };
                }
            }
            catch(Exception ex)
            {                                                                   // handle unexpected exception
                reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
            }

            e.Reply(status, reply?.ToJsonString());                             // send response
            return true;
        }


        /// <summary>Edits a thread.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE.</returns>
        private static bool _Edit(HttpSvrEventArgs e)
        {
            JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
            int status = HttpStatusCode.BAD_REQUEST;                            // initialize response

            try
            {
                JsonNode? json = JsonNode.Parse(e.Payload);                     // parse payload
                if(json != null)
                {
                    try
                    {
                        Thread th = Thread.ByID(Convert.ToInt32(e.Path[9..]));
                        th.BeginEdit(Session.From(e));                          // edit thread with session

                        th.Title = (string?) json["title"] ?? string.Empty;
                        th.Save();                                              // save thread

                        th.EndEdit();                                           // end editing

                        status = HttpStatusCode.OK;
                        reply = new JsonObject() { ["success"] = true, ["message"] = "Thread edited." };
                    }
                    catch(SecurityException ex)
                    {
                        reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
                    }
                    catch(Exception)
                    {                                                           // thread not found
                        status = HttpStatusCode.NOT_FOUND;
                        reply = new JsonObject() { ["success"] = false, ["message"] = "Thread not found." };
                    }
                }
            }
            catch(Exception ex)
            {                                                                   // handle unexpected exception
                reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
            }

            e.Reply(status, reply?.ToJsonString());                             // send response
            return true;
        }


        /// <summary>Deletes a thread.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE.</returns>
        private static bool _Delete(HttpSvrEventArgs e)
        {
            JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
            int status = HttpStatusCode.BAD_REQUEST;                            // initialize response

            try
            {
                try
                {
                    Thread th = Thread.ByID(Convert.ToInt32(e.Path[9..]));
                    th.BeginEdit(Session.From(e));                          // edit thread with session

                    th.Delete();                                            // save thread

                    th.EndEdit();                                           // end editing

                    status = HttpStatusCode.OK;
                    reply = new JsonObject() { ["success"] = true, ["message"] = "Thread deleted." };
                }
                catch(SecurityException ex)
                {
                    reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
                }
                catch(Exception)
                {                                                           // thread not found
                    status = HttpStatusCode.NOT_FOUND;
                    reply = new JsonObject() { ["success"] = false, ["message"] = "Thread not found." };
                }
            }
            catch(Exception ex)
            {                                                                   // handle unexpected exception
                reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
            }

            e.Reply(status, reply?.ToJsonString());                             // send response
            return true;
        }


        /// <summary>Queries a thread.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE.</returns>
        private static bool _Query(HttpSvrEventArgs e)
        {
            JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
            int status = HttpStatusCode.BAD_REQUEST;                            // initialize response

            try
            {
                try
                {
                    Thread th = Thread.ByID(Convert.ToInt32(e.Path[9..]));
                        
                    status = HttpStatusCode.OK;
                    reply = new JsonObject() { ["success"] = true, ["id"] = th.ID, ["title"] = th.Title, ["owner"] = th.Owner, ["time"] = th.Time };
                }
                catch(Exception)
                {                                                               // thread not found
                    status = HttpStatusCode.NOT_FOUND;
                    reply = new JsonObject() { ["success"] = false, ["message"] = "Thread not found." };
                }
            }
            catch(Exception ex)
            {                                                                   // handle unexpected exception
                reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
            }

            e.Reply(status, reply?.ToJsonString());                             // send response
            return true;
        }


        /// <summary>Gets the entries for a thread.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE.</returns>
        private static bool _GetEntries(HttpSvrEventArgs e)
        {
            JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
            int status = HttpStatusCode.BAD_REQUEST;                            // initialize response

            try
            {
                try
                {
                    Thread th = Thread.ByID(Convert.ToInt32(e.Path.TrimEnd('/')[..^8][9..]));
                        
                    status = HttpStatusCode.OK;
                    JsonArray arr = new JsonArray();
                    foreach(Entry i in th.Entries)
                    {
                        arr.Add(new JsonObject() { ["id"] = i.ID, ["text"] = i.Text, ["owner"] = i.Owner, ["thread"] = th.ID, ["time"] = i.Time });
                    }
                    reply = new JsonObject() { ["success"] = true, ["elements"] =  arr};
                }
                catch(Exception)
                {                                                               // thread not found
                    status = HttpStatusCode.NOT_FOUND;
                    reply = new JsonObject() { ["success"] = false, ["message"] = "Thread not found." };
                }
            }
            catch(Exception ex)
            {                                                                   // handle unexpected exception
                reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
            }

            e.Reply(status, reply?.ToJsonString());                             // send response
            return true;
        }
    }
}
