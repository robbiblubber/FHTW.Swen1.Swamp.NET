using System;



namespace FHTW.Swen1.Swamp
{
    public interface IHandler
    {
        public bool Handle(HttpSvrEventArgs e);
    }
}
