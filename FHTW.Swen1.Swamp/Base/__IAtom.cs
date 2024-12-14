using System;
using FHTW.Swen1.Swamp.Security;



namespace FHTW.Swen1.Swamp.Base
{
    /// <summary>Persistent objects implement this interface.</summary>
    /// <remarks>This interface provides internal information about an object.</remarks>
    public interface __IAtom
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the internal ID of an object.</summary>
        public object? __InternalID { get; set; }


        /// <summary>Gets the editing user for this object.</summary>
        public User? __EditingUser { get; }
    }
}
