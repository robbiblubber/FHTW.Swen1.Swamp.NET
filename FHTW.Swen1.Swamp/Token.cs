using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class provides methods for the token-based security.</summary>
    public static class Token
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private constants                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Alphabet string.</summary>
        private const string _ALPHABET = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Token dictionary.</summary>
        internal static Dictionary<string, User> _Tokens = new();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new token for a user.</summary>
        /// <param name="user">User.</param>
        /// <returns>Token string.</returns>
        internal static string _CreateTokenFor(User user)
        {
            string rval = string.Empty;
            Random rnd = new();

            for(int i = 0; i < 24; i++)
            {
                rval += _ALPHABET[rnd.Next(0, 62)];
            }

            _Tokens.Add(rval, user);

            return rval;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Authenticates a user by token.</summary>
        /// <param name="token">Token string.</param>
        /// <returns>Returns a tupple of success flag and user object.
        ///          If successful, the success flag is TRUE and the user represents the authenticated user,
        ///          otherwise success flag if FALSE and user object is NULL.</returns>
        public static (bool Success, User? User) Authenticate(string token)
        {
            if(Program.ALLOW_DEBUG_TOKEN && token.EndsWith("-debug"))
            {                                                                   // accept debug token
                token = token[..^6];
                User? user = User.Get(token);

                return ((user != null), user);
            }

            if(_Tokens.ContainsKey(token))
            {                                                                   // find real token
                return (true, _Tokens[token]);
            }

            return (false, null);
        }


        /// <summary>Authenticates a user by token.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns a tupple of success flag and user object.
        ///          If successful, the success flag is TRUE and the user represents the authenticated user,
        ///          otherwise success flag if FALSE and user object is NULL.</returns>
        public static (bool Success, User? User) Authenticate(HttpSvrEventArgs e)
        {
            foreach(HttpHeader i in e.Headers)
            {                                                                   // iterates headers
                if(i.Name == "Authorization")
                {                                                               // found "Authorization" header
                    if(i.Value[..7] == "Bearer ")
                    {                                                           // needs to start with "Bearer "
                        return Authenticate(i.Value[7..].Trim());               // authenticate by token
                    }
                    break;
                }
            }
            
            return (false, null);                                               // "Authorization" header not found, authentication failed
        }
    }
}
