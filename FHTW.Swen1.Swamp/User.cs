using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class represents a user.</summary>
    public sealed class User
    {
        private static Dictionary<string, User> _Users = new();



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
        public void Save(string token)
        {
            (bool Success, User? User) auth = Token.Authenticate(token);
            if(auth.Success)
            {
                if(auth.User!.UserName != UserName)
                {
                    throw new UserException("Trying to change other user's data.");
                }
                // Save data.
            }
            else { new UserException("Not authenticated."); }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public static void Create(string userName, string password, string fullName = "", string eMail = "")
        {
            if(_Users.ContainsKey(userName))
            {
                throw new UserException("User name already taken.");
            }

            User user = new()
            {
                UserName = userName,
                FullName = fullName,
                EMail = eMail
            };
            _Users.Add(user.UserName, user);
        }


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
