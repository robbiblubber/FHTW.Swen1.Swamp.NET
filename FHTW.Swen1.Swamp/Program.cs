﻿using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace FHTW.Swen1.Swamp
{
    internal class Program
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // entry point                                                                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Main entry point.</summary>
        /// <param name="args">Arguments.</param>
        static void Main(string[] args)
        {
            /*HttpSvr svr = new();
            svr.Incoming += _ProcessMesage;

            svr.Run();*/

            string s = "{\"id\":\"5\", \"x\":\"x\"}";
            JsonNode j = JsonNode.Parse(s);
            Console.WriteLine(j["id"]);
            int z = 9;
        }


        /// <summary>Event handler for incoming server requests.</summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event arguments.</param>
        private static void _ProcessMesage(object sender, HttpSvrEventArgs e)
        {
            Console.WriteLine(e.PlainMessage);

            e.Reply(200, "Yo! Understood.");
        }
    }
}