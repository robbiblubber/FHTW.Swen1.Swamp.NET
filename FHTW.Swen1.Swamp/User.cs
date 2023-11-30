using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class represents a user.</summary>
    public sealed class User: IItem
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Repository.</summary>
        private static UserRepository _Repository = new UserRepository();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets a user by ID.</summary>
        /// <param name="id">ID.</param>
        /// <returns>User.</returns>
        public static User ByID(string id)
        {
            User? rval = _Repository.Get(id);

            if(rval == null) { throw new Exception("User not found."); }
            return rval;
        }


        /// <summary>Cretaes a new user object.</summary>
        /// <param name="id">ID.</param>
        /// <param name="name">Name.</param>
        /// <param name="password">Password.</param>
        /// <returns>Returns the newly created user object.</returns>
        public User Create(string id, string name, string password)
        {
            return _Repository.Create(id, name, password);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the user ID.</summary>
        public string ID
        {
            get; internal set;
        } = "";
        
        
        /// <summary>Gets or sets the user name.</summary>
        public string Name
        {
            get; set;
        } = "";



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] IItem                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the object ID.</summary>
        string IItem.ID 
        { 
            get { return ID; }
        }


        /// <summary>Saves the object.</summary>
        public void Save()
        {
            _Repository.Save(this);
        }


        /// <summary>Deletes the object.</summary>
        public void Delete()
        {
            _Repository.Delete(this);
        }
    }
}
