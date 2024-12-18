using System;
using FHTW.Swen1.Swamp.Security;



namespace FHTW.Swen1.Swamp.Base
{
    /// <summary>Persistent objects implement this interface.</summary>
    public interface IAtom : __IAtom
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Starts editing an object.</summary>
        /// <param name="session">Session that performs the operation.</param>
        public void BeginEdit(Session session);


        /// <summary>Ends editing an object.</summary>
        public void EndEdit();


        /// <summary>Deletes the object.</summary>
        public void Delete();


        /// <summary>Saves the object.</summary>
        public void Save();


        /// <summary>Refrehes the object.</summary>
        public void Refresh();
    }
}
