using System;
using System.Security;

using FHTW.Swen1.Swamp.Base;
using FHTW.Swen1.Swamp.Repositories;

using Thread = FHTW.Swen1.Swamp.Model.Thread;



namespace FHTW.Swen1.Swamp.Model
{
    /// <summary>This class represents an entry.</summary>
    public sealed class Entry: Atom, IAtom, __IAtom
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>User repository.</summary>
        private static EntryRepository _Repository = new();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        public Entry()
        {}



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the entry ID.</summary>
        public int ID
        {
            get { return (int?) _InternalID ?? -1; }
        }


        /// <summary>Gets or sets the entry text.</summary>
        public string Text
        {
            get; set;
        } = string.Empty;


        /// <summary>Gets or sets the entry time.</summary>
        public DateTime Time
        {
            get; internal set;
        } = DateTime.Now;


        /// <summary>Gets the thread.</summary>
        public Thread? Thread
        {
            get; internal set;
        }


        /// <summary>Gets the entry owner user name.</summary>
        public string Owner
        {
            get; internal set;
        } = string.Empty;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets an entry by its ID.</summary>
        /// <param name="id">ID.</param>
        /// <returns>Returns the entry with the given ID.</returns>
        public static Entry ByID(int id)
        {
            return _Repository.Get(id);
        }


        /// <summary>Gets the entries for a thread.</summary>
        /// <param name="thread">Thread.</param>
        /// <returns>Returns a set of entries that belong to the given thread.</returns>
        public static IEnumerable<Entry> For(Thread thread)
        {
            return _Repository.GetFor(thread);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Atom                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Deletes the object.</summary>
        public override void Delete()
        {
            if(!(_EditingSession?.Valid ?? false)) { throw new SecurityException("Required authentication."); }
            if(!(_EditingSession.Is(Owner) || _EditingSession.IsAdmin))
            {
                throw new SecurityException("Only owner or admins can delete entry.");
            }

            _Repository.Delete(this);
        }


        /// <summary>Saves the object.</summary>
        public override void Save()
        {
            if(!(_EditingSession?.Valid ?? false)) { throw new SecurityException("Required authentication."); }

            if(_InternalID is null)
            {
                Owner = _EditingSession!.User!.UserName;
            }
            else if(!(_EditingSession.Is(Owner) || _EditingSession.IsAdmin))
            {
                throw new SecurityException("Only owner or admins can edit entry.");
            }

            _Repository.Save(this);
        }


        /// <summary>Refrehes the object.</summary>
        public override void Refresh()
        {
            _Repository.Refresh(this);
        }
    }
}
