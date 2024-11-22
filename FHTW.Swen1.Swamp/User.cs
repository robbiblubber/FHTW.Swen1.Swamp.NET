using System;
using System.Security;
using System.Security.Authentication;

using FHTW.Swen1.Swamp.Exceptions;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class represents a user.</summary>
    public sealed class User
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Currently holds the system users.</summary>
        /// <remarks>Is to be removed by database implementation later.</remarks>
        private static Dictionary<string, User> _Users = new();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        private User()
        {}



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the user name.</summary>
        public string UserName
        {
            get; private set;
        } = string.Empty;


        /// <summary>Gets or sets the user's full name.</summary>
        public string FullName
        {
            get; set;
        } = string.Empty;


        /// <summary>Gets or sets the user's e-mail address.</summary>
        public string EMail
        {
            get; set;
        } = string.Empty;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Saves changes to the user object.</summary>
        /// <param name="token">Token of the session trying to modify the object.</param>
        /// <exception cref="SecurityException">Thrown in case of an unauthorized attempt to modify data.</exception>
        /// <exception cref="AuthenticationException">Thrown when the token is invalid.</exception>
        public void Save(string token)
        {
            (bool Success, User? User) auth = Token.Authenticate(token);
            if(auth.Success)
            {
                if(auth.User!.UserName != UserName)
                {
                    throw new SecurityException("Trying to change other user's data.");
                }
                // Save data.
            }
            else { new AuthenticationException("Not authenticated."); }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a user.</summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="fullName">Full name.</param>
        /// <param name="eMail">E-mail addresss.</param>
        /// <exception cref="UserException">Thrown when the user name already exists.</exception>
        public static void Create(string userName, string password, string fullName = "", string eMail = "")
        {
            if(_Users.ContainsKey(userName))
            {
                throw new UserException("User name already exists.");
            }

            User user = new()
            {
                UserName = userName,
                FullName = fullName,
                EMail = eMail
            };

            _Users.Add(user.UserName, user);
        }


        /// <summary>Performs a user logon.</summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <returns>Returns a tuple of success flag and token.
        ///          If successful, the success flag is TRUE and the token contains a token string,
        ///          otherwise success flag is FALSE and token is empty.</returns>
        public static (bool Success, string Token) Logon(string userName, string password)
        {
            if(_Users.ContainsKey(userName))
            {
                return (true, Token._CreateTokenFor(_Users[userName]));
            }

            return (false, string.Empty);
        }
    }
}
