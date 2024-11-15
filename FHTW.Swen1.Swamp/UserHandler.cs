using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(e.Path.StartsWith("users"))
            {
                // bla...

                //e.Reply()
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
