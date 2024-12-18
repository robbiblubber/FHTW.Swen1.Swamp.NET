using System;

using FHTW.Swen1.Swamp.Server;



namespace FHTW.Swen1.Swamp.Security
{
    /// <summary>This class represents a session.</summary>
    public sealed class Session
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private constants                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Alphabet string.</summary>
        private const string _ALPHABET = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>Invalid session.</summary>
        private static readonly Session _INVALID = new Session(null);



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Session dictionary.</summary>
        internal static Dictionary<string, Session> _Sessions = new();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="user">User.</param>
        private Session(User? user) 
        {
            if((User = user) != null)                                           // set user
            {
                Random rnd = new();                                             // create token
                for(int i = 0; i < 24; i++) { Token += _ALPHABET[rnd.Next(0, 62)]; }

                _Sessions.Add(Token, this);                                     // register token
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a session by user name and password.</summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <returns>Returns a session object.</returns>
        public static Session Create(string userName, string password)
        {
            try
            {
                User user = User.ByUserName(userName);                          // get user and verify password
                if(user.VerifyPassword(password)) { return new Session(user); }
            }
            catch(Exception) {}

            return _INVALID;
        }


        /// <summary>Creates a session by its token.</summary>
        /// <param name="token">Token.</param>
        /// <returns>Returns a session object.</returns>
        public static Session Create(string token)
        {
            if(Program.ALLOW_DEBUG_TOKEN && token.EndsWith("-debug"))
            {                                                                   // accept debug token
                try
                {
                    return new(User.ByUserName(token[..^6]));                   // get user by user name
                }
                catch(Exception) {}
            }

            if(_Sessions.ContainsKey(token)) { return _Sessions[token]; }       // get session from stored sessions
            return _INVALID;
        }


        /// <summary>Creates a session by authentication header.</summary>
        /// <param name="e">Http Server event arguments.</param>
        /// <returns>Returns a session object.</returns>
        public static Session From(HttpSvrEventArgs e)
        {
            foreach (HttpHeader i in e.Headers)
            {                                                                   // iterates headers
                if(i.Name == "Authorization")
                {                                                               // found "Authorization" header
                    if(i.Value[..7] == "Bearer ")
                    {                                                           // needs to start with "Bearer "
                        return Create(i.Value[7..].Trim());                     // authenticate by token
                    }
                    break;
                }
            }

            return _INVALID;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets if the session represents a user.</summary>
        /// <param name="user">User.</param>
        /// <returns>Returns TRUE if the session user is the given user, otherwise returns FALSE.</returns>
        public bool Is(User user)
        {
            return Is(user.UserName);
        }


        /// <summary>Gets if the session represents a user.</summary>
        /// <param name="user">User name.</param>
        /// <returns>Returns TRUE if the session user is the given user name, otherwise returns FALSE.</returns>
        public bool Is(string userName)
        {
            return ((User?.UserName ?? string.Empty) == userName);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the session user.</summary>
        public User? User
        {
            get; private set;
        }


        /// <summary>Gets the session token.</summary>
        public string Token
        {
            get; private set;
        } = string.Empty;


        /// <summary>Gets if the session is valid.</summary>
        public bool Valid
        {
            get { return (User is not null); }
        }


        /// <summary>Gets if the session has administrative privileges.</summary>
        public bool IsAdmin
        {
            get { return User?.IsAdmin ?? false; }
        }
    }
}
