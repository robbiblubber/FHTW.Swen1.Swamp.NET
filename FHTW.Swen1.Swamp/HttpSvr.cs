using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;



namespace FHTW.Swen1.Swamp
{
    public sealed class HttpSvr
    {
        private TcpListener? _Listener;


        public event HttpSvrEventHandler? Incoming;


        public bool Active
        {
            get; set;
        } = false;


        public void Run()
        {
            if(Active) return;

            Active = true;
            _Listener = new(IPAddress.Parse("127.0.0.1"), 12000);
            _Listener.Start();

            byte[] buf = new byte[256];

            while(Active)
            {
                TcpClient client = _Listener.AcceptTcpClient();
                string data = string.Empty;
                
                while(client.GetStream().DataAvailable || string.IsNullOrWhiteSpace(data))
                {
                    int n = client.GetStream().Read(buf, 0, buf.Length);
                    data += Encoding.ASCII.GetString(buf, 0, n);
                }

                Incoming?.Invoke(this, new(client, data));
            }
        }
    }
}
