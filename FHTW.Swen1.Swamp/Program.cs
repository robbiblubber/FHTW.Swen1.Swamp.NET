using System.Net.Http.Json;
using System.Runtime.CompilerServices;
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
            HttpSvr svr = new();
            svr.Incoming += _ProcessMesage;

            svr.Run();
        }


        /// <summary>Event handler for incoming server requests.</summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event arguments.</param>
        private async static void _ProcessMesage(object sender, HttpSvrEventArgs e)
        {
            await Task.Run(() =>
            {
                Console.WriteLine(e.PlainMessage);
                e.Reply(200, "Yo! Understood.");
            });
        }
    }
}