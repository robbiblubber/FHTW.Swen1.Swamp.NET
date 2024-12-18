using System;
using System.Security;
using System.Text.Json.Nodes;

using Thread = FHTW.Swen1.Swamp.Model.Thread;

using FHTW.Swen1.Swamp.Security;
using FHTW.Swen1.Swamp.Server;
using FHTW.Swen1.Swamp.Model;



namespace FHTW.Swen1.Swamp.Handlers
{
    /// <summary>This class implements a handler for entries.</summary>
    public sealed class EntryHandler: Handler, IHandler
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Handler                                                                                               //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Handles an incoming HTTP request.</summary>
        /// <param name="e">Event arguments.</param>
        public override bool Handle(HttpSvrEventArgs e)
        {
            if(e.Path.TrimEnd('/', ' ', '\t') == "/entries" && e.Method == "POST")
            {
                return _Create(e);
            }
            else if(e.Path.StartsWith("/entries/"))
            {
                if(e.Method == "GET")
                {
                    return _Query(e);
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

        /// <summary>Creates an entry.</summary>
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
                    Entry en = new() { Text = (string?) json["text"] ?? string.Empty, Thread = Thread.ByID((int?) json["thread"] ?? -1) };
                    en.BeginEdit(Session.From(e));                              // edit entry with session

                    en.Save();                                                  // save entry
                    
                    en.EndEdit();                                               // end editing

                    status = HttpStatusCode.OK;
                    reply = new JsonObject() { ["success"] = true,
                                               ["id"] = en.ID };
                }
            }
            catch(Exception ex)
            {                                                                   // handle unexpected exception
                reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
            }

            e.Reply(status, reply?.ToJsonString());                             // send response
            return true;
        }


        /// <summary>Edits an entry.</summary>
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
                        Entry en = Entry.ByID(Convert.ToInt32(e.Path[9..]));
                        en.BeginEdit(Session.From(e));                          // edit entry with session

                        en.Text = (string?) json["text"] ?? string.Empty;
                        en.Save();                                              // save entry

                        en.EndEdit();                                           // end editing

                        status = HttpStatusCode.OK;
                        reply = new JsonObject() { ["success"] = true, ["message"] = "Entry edited." };
                    }
                    catch(SecurityException ex)
                    {
                        reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
                    }
                    catch(Exception)
                    {                                                           // entry not found
                        status = HttpStatusCode.NOT_FOUND;
                        reply = new JsonObject() { ["success"] = false, ["message"] = "Entry not found." };
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


        /// <summary>Deletes an entry.</summary>
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
                    Entry en = Entry.ByID(Convert.ToInt32(e.Path[9..]));
                    en.BeginEdit(Session.From(e));                          // edit entry with session

                    en.Delete();                                            // save entry

                    en.EndEdit();                                           // end editing

                    status = HttpStatusCode.OK;
                    reply = new JsonObject() { ["success"] = true, ["message"] = "Entry deleted." };
                }
                catch(SecurityException ex)
                {
                    reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
                }
                catch(Exception)
                {                                                           // entry not found
                    status = HttpStatusCode.NOT_FOUND;
                    reply = new JsonObject() { ["success"] = false, ["message"] = "Entry not found." };
                }
            }
            catch(Exception ex)
            {                                                                   // handle unexpected exception
                reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
            }

            e.Reply(status, reply?.ToJsonString());                             // send response
            return true;
        }


        /// <summary>Queries an entry.</summary>
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
                    Entry en = Entry.ByID(Convert.ToInt32(e.Path[9..]));
                        
                    status = HttpStatusCode.OK;
                    reply = new JsonObject() { ["success"] = true, ["id"] = en.ID, ["text"] = en.Text, ["owner"] = en.Owner, ["thread"] = en.Thread?.ID ?? -1, ["time"] = en.Time };
                }
                catch(Exception)
                {                                                               // entry not found
                    status = HttpStatusCode.NOT_FOUND;
                    reply = new JsonObject() { ["success"] = false, ["message"] = "Entry not found." };
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
