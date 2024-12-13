using System;



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
            User u;
            /*u = new User();
            u.UserName = "alex";
            u.FullName = "Alexander Lang";
            u.EMail = "langa@technikum-wien.at";

            u.Save();
            */

            u = User.ByUserName("alex");
            u.EMail = "thedevil@hell.org";

            u.Refresh();

            return;
            HttpSvr svr = new();
            svr.Incoming += Svr_Incoming; //(sender, e) => { Handler.HandleEvent(e); };

            svr.Run();
        }



        private static void Svr_Incoming(object sender, HttpSvrEventArgs e)
        {
            _ = Handler.HandleEvent(e);

            /*           
            Console.WriteLine(e.Method);
            Console.WriteLine(e.Path);
            Console.WriteLine();
            foreach(HttpHeader i in e.Headers) 
            {
                Console.WriteLine(i.Name + ": " + i.Value);
            }
            Console.WriteLine();
            Console.WriteLine(e.Payload);

            e.Reply(HttpStatusCode.OK, "Yo Baby!");
            */
        }
    }
}