using System;
using System.Net.Sockets;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class defines event arguments for the <see cref="HttpSvrEventHandler"/> event handler.</summary>
    public class HttpSvrEventArgs: EventArgs
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected members                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>TCP client.</summary>
        protected TcpClient _Client;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="client">TCP client.</param>
        /// <param name="plainMessage">Plain HTTP message.</param>
        public HttpSvrEventArgs(TcpClient client, string plainMessage) 
        {
            _Client = client;

        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the plain message.</summary>
        public string PlainMessage
        {
            get; protected set;
        } = string.Empty;


        /// <summary>Gets the HTTP method.</summary>
        public virtual string Method
        {
            get; protected set;
        } = string.Empty;


        /// <summary>Gets the HTTP path.</summary>
        public virtual string Path
        {
            get; protected set;
        } = string.Empty;


        /// <summary>Gets the HTTP headers.</summary>
        public virtual HttpHeader[] Headers
        {
            get; protected set;
        } = Array.Empty<HttpHeader>();


        /// <summary>Gets the payload.</summary>
        public virtual string Payload
        {
            get; protected set;
        } = string.Empty;
    }
}
