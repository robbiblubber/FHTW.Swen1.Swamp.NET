using System;

using FHTW.Swen1.Swamp.Repositories;
using FHTW.Swen1.Swamp.Security;



namespace FHTW.Swen1.Swamp.Base
{
    /// <summary>This class provides a base implementation of the IAtom interface.</summary>
    public abstract class Atom: IAtom, __IAtom
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected members                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Editing user.</summary>
        protected User? _EditingUser = null;

        /// <summary>Internal ID.</summary>
        protected object? _InternalID = null;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected methods                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Sets the object ID.</summary>
        /// <param name="id">ID.</param>
        protected virtual void _SetID(object? id)
        {
            _InternalID = id;
        }

                

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] IAtom                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Starts editing an object.</summary>
        /// <param name="user">User that performs the operation.</param>
        public virtual void Edit(User user)
        {
            _EditingUser = user;
        }


        /// <summary>Deletes the object.</summary>
        public abstract void Delete();


        /// <summary>Saves the object.</summary>
        public abstract void Save();


        /// <summary>Refrehes the object.</summary>
        public abstract void Refresh();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] __IAtom                                                                                              //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the internal ID of an object.</summary>
        object? __IAtom.__InternalID
        {
            get { return _InternalID; }
            set { _SetID(value); }
        }


        /// <summary>Gets the editing user for this object.</summary>
        User? __IAtom.__EditingUser 
        {
            get { return _EditingUser; }
        }
    }
}
