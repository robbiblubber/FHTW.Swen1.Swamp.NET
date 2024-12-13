using System;
using System.Security;
using System.Security.Authentication;

using FHTW.Swen1.Swamp.Exceptions;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class represents a user.</summary>
    public sealed class User: IAtom, __IAtom
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>User repository.</summary>
        private static UserRepository _Repository = new();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private members                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Editing user.</summary>
        private User? _EditingUser = null;

        /// <summary>User name.</summary>
        internal string _UserName = string.Empty;

        /// <summary>Admin flag.</summary>
        internal bool _IsAdmin = false;

        /// <summary>Internal ID.</summary>
        internal object? _InternalID = null;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        public User()
        {}



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static properties                                                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets a user by user name.</summary>
        /// <param name="userName">User name.</param>
        /// <returns>User.</returns>
        public static User ByUserName(string userName)
        {
            return _Repository.Get(userName);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the user name.</summary>
        public string UserName
        {
            get { return _UserName; }
            set 
            { 
                if(!string.IsNullOrEmpty(_UserName)) { throw new SecurityException("Changing user name disallowed."); }
                _UserName = value; 
            }
        }


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


        /// <summary>Gets or sets if the user is administrator.</summary>
        public bool IsAdmin
        {
            get { return _IsAdmin; }
            set
            {
                if(_IsAdmin != value)
                {
                    if((_EditingUser is null) || (!_EditingUser.IsAdmin))
                    {
                        throw new SecurityException("Administrative access required.");
                    }
                    _IsAdmin = value;
                }
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] IAtom                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Starts editing an object.</summary>
        /// <param name="user">User that performs the operation.</param>
        public void Edit(User user)
        {
            _EditingUser = user;
        }


        /// <summary>Deletes the object.</summary>
        public void Delete()
        {
            if((_EditingUser != this) && (!(_EditingUser?.IsAdmin ?? false)))
            {
                throw new SecurityException("Administrative access required.");
            }
            _Repository.Delete(this);
        }


        /// <summary>Saves the object.</summary>
        public void Save()
        {
            if((((__IAtom) this).__InternalID == null) && IsAdmin)
            {
                throw new SecurityException("Creating admin user disallowed.");
            }
            if(string.IsNullOrEmpty(_UserName)) throw new SecurityException("Creating user with empty user name disallowed.");

            _Repository.Save(this);
        }


        /// <summary>Refrehes the object.</summary>
        public void Refresh()
        {
            _Repository.Refresh(this);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] __IAtom                                                                                              //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets or sets the internal ID.</summary>
        object? __IAtom.__InternalID
        {
            get { return _InternalID; }
            set { _UserName = (string?) (_InternalID = value) ?? string.Empty; }
        }


        /// <summary>Gets the editing user for this object.</summary>
        User? __IAtom.__EditingUser 
        { 
            get { return _EditingUser; }
        }
    }
}
