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
        private static string _ALPHABET = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";



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
            if(_Tokens.ContainsKey(token))
            {
                return (true, _Tokens[token]);
            }

            return (false, null);
        }
    }
}
