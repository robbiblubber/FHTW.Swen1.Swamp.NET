using System;
using System.Security;

using FHTW.Swen1.Swamp.Base;
using FHTW.Swen1.Swamp.Repositories;
using FHTW.Swen1.Swamp.Security;



namespace FHTW.Swen1.Swamp.Model
{
    /// <summary>This class represents a thread.</summary>
    public sealed class Thread: Atom, IAtom, __IAtom
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>User repository.</summary>
        private static ThreadRepository _Repository = new();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        public Thread()
        {}


        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="user">Creating user.</param>
        internal Thread(User user)
        {
            Edit(user);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the thread ID.</summary>
        public int ID
        {
            get { return (int?) _InternalID ?? -1; }
        }


        /// <summary>Gets or sets the thread title.</summary>
        public string Title
        {
            get; set;
        } = string.Empty;


        /// <summary>Gets or sets the thread time.</summary>
        public DateTime Time
        {
            get; set;
        } = DateTime.Now;


        /// <summary>Gets the thread owner user name.</summary>
        public string Owner
        {
            get; internal set;
        } = string.Empty;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Atom                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Deletes the object.</summary>
        public override void Delete()
        {
            if(_EditingUser == null) { throw new SecurityException("No user."); }
            if(!_EditingUser.IsAdmin) { throw new SecurityException("Only admins may delete threads."); }
            
            _Repository.Delete(this);
        }


        /// <summary>Saves the object.</summary>
        public override void Save()
        {
            if(_EditingUser == null) { throw new SecurityException("No user."); }

            _Repository.Save(this);
        }


        /// <summary>Refrehes the object.</summary>
        public override void Refresh()
        {
            _Repository.Refresh(this);
        }
    }
}
