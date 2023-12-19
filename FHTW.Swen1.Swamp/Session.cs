using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class represents a session.</summary>
    public class Session
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Synchronization object.</summary>
        private static object _Sync = new object();

        /// <summary>Session list.</summary>
        private static Dictionary<string, Session> _Sessions = new();

        /// <summary>Token buffer.</summary>
        internal const string _TOKEN_BUFFER = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this object.</summary>
        private Session() 
        {}



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // puprivateblic static methods                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a token.</summary>
        /// <returns>Token.</returns>
        private static string _CreateToken()
        {
            string rval = string.Empty;
            Random rnd = new Random();

            while(true)
            {
                for(int i = 0; i < 16; i++)
                {
                    rval += _TOKEN_BUFFER[rnd.Next(0, _TOKEN_BUFFER.Length - 1)];
                }

                if(!_Sessions.ContainsKey(rval)) { break; }
            }

            return rval;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new session.</summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <returns>Session.</returns>
        public static Session Create(string userName, string password)
        {
            Session rval = new Session();
            User user = User.ByID(userName);

            if(user != null) 
            {
                if(user.VerifyPassword(password))
                {
                    rval.User = user;
                    rval.Token = _CreateToken();

                    lock(_Sync) { _Sessions.Add(rval.Token, rval); }
                }
            }

            return rval;
        }


        /// <summary>Returns a session for a token.</summary>
        /// <param name="token">Token.</param>
        /// <returns>Session object.</returns>
        public static Session FromToken(string token)
        {
            if(!_Sessions.ContainsKey(token))
            {
                return new Session() { Token = token };
            }

            lock(_Sync)
            {
                return _Sessions[token];
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the session token.</summary>
        public string Token
        {
            get; private set;
        } = "";


        /// <summary>Gets the session user.</summary>
        public User? User
        {
            get; private set;
        }


        /// <summary>Gets if the session is valid.</summary>
        public bool Valid
        {
            get { return (User != null); }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Closes the session-</summary>
        public void Close()
        {
            User = null;

            lock(_Sync)
            {
                if(_Sessions.ContainsKey(Token)) { _Sessions.Remove(Token); }
            }
        }
    }
}
