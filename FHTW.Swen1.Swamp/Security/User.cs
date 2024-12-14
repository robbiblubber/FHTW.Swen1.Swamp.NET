using System;
using System.Security;

using FHTW.Swen1.Swamp.Base;
using FHTW.Swen1.Swamp.Repositories;



namespace FHTW.Swen1.Swamp.Security
{
    /// <summary>This class represents a user.</summary>
    public sealed class User: Atom, IAtom, __IAtom
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>User repository.</summary>
        private static UserRepository _Repository = new();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private members                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>User name.</summary>
        private string _UserName = string.Empty;

        /// <summary>Admin flag.</summary>
        internal bool _IsAdmin = false;



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
                if (!string.IsNullOrEmpty(_UserName)) { throw new SecurityException("Changing user name disallowed."); }
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
                    if(_EditingUser is null || !_EditingUser.IsAdmin)
                    {
                        throw new SecurityException("Administrative access required.");
                    }
                    _IsAdmin = value;
                }
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Atom                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Sets the object ID.</summary>
        /// <param name="id">ID.</param>
        protected override void _SetID(object? id)
        {
            base._SetID(id);
            _UserName = (string?) id ?? string.Empty;
        }


        /// <summary>Deletes the object.</summary>
        public override void Delete()
        {
            if (_EditingUser != this && !(_EditingUser?.IsAdmin ?? false))
            {
                throw new SecurityException("Administrative access required.");
            }
            
            _Repository.Delete(this);
        }


        /// <summary>Saves the object.</summary>
        public override void Save()
        {
            if((((__IAtom) this).__InternalID == null) && IsAdmin)
            {
                throw new SecurityException("Creating admin user disallowed.");
            }
            if(string.IsNullOrEmpty(_UserName)) throw new SecurityException("Empty user name disallowed.");

            _Repository.Save(this);
        }


        /// <summary>Refrehes the object.</summary>
        public override void Refresh()
        {
            _Repository.Refresh(this);
        }
    }
}
