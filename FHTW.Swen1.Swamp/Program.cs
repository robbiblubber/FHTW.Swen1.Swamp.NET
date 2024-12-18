using System;
using FHTW.Swen1.Swamp.Handlers;
using FHTW.Swen1.Swamp.Security;
using FHTW.Swen1.Swamp.Server;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class contains the main entry point of the application.</summary>
    internal class Program
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public constants                                                                                                 //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Determines if debug token ("UserName-debug") will be accepted.</summary>
        public const bool ALLOW_DEBUG_TOKEN = true;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // entry point                                                                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Application entry point.</summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            HttpSvr svr = new();
            svr.Incoming += Svr_Incoming;

            svr.Run();
        }



        private static void Svr_Incoming(object sender, HttpSvrEventArgs e)
        {
            _ = Handler.HandleEvent(e);
        }
    }
}