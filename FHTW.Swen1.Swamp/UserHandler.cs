using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHTW.Swen1.Swamp
{
    public class UserHandler: Handler, IHandler
    {
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
