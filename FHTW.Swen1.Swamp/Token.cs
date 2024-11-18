using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class provides methods for the token-based security.</summary>
    public static class Token
    {
        internal static Dictionary<string, User> _Tokens = new();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private constants                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Alphabet string.</summary>
        private static string _ALPHABET = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";



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
