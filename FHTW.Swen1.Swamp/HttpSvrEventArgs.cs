using System;
using System.Net.Http.Headers;
using System.Net.Sockets;

namespace FHTW.Swen1.Swamp
{
    public class HttpSvrEventArgs: EventArgs
    {
        protected TcpClient _Client;


        public HttpSvrEventArgs(TcpClient client, string plainMessage) 
        {
            _Client = client;

        }


        public string PlainMessage
        {
            get; protected set;
        } = string.Empty;


        public virtual string Method
        {
            get; protected set;
        } = string.Empty;


        public virtual string Path
        {
            get; protected set;
        } = string.Empty;


        public virtual HttpHeader[] Headers
        {
            get; protected set;
        } = Array.Empty<HttpHeader>();


        public virtual string Payload
        {
            get; protected set;
        } = string.Empty;
    }
}
