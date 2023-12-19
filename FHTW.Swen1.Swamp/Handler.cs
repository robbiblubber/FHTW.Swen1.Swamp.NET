using System;
using System.Reflection;



namespace FHTW.Swen1.Swamp
{
    /// <summary>Provides an abstract implementation of the IHandler interface.</summary>
    public abstract class Handler: IHandler
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Handlers.</summary>
        private static List<IHandler>? _Handlers = null;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets a list of handlers.</summary>
        /// <returns>List of handlers.</returns>
        private static List<IHandler> _GetHandlers()
        {
            List<IHandler> rval = new();
            foreach(Type i in Assembly.GetExecutingAssembly().GetTypes().Where(m => (!m.IsAbstract) && m.IsAssignableTo(typeof(IHandler))))
            {
                IHandler? h = (IHandler?) Activator.CreateInstance(i);
                if(h != null) { rval.Add(h); }
            }
            return rval;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Discovers and uses a handler to hanlde a request.</summary>
        /// <param name="e">Event arguments.</param>
        public static void HandleEvent(HttpSvrEventArgs e)
        {
            if(_Handlers == null ) { _Handlers = _GetHandlers(); }

            foreach(IHandler i in _Handlers)
            {
                if(i.Handle(e)) return;
            }

            e.Reply(HttpCodes.BAD_REQUEST);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] IHandler                                                                                             //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Checks if the handler can handle a request and handles the request if it does.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE if the event has been handled, otherwise returns FALSE.</returns>
        public abstract bool Handle(HttpSvrEventArgs e);
    }
}
