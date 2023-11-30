using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FHTW.Swen1.Swamp
{
    /// <summary>Business items implement this interface.</summary>
    public interface IItem
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // properties                                                                                                       //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the object ID.</summary>
        public string ID { get; }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // methods                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Saves the object.</summary>
        public void Save();


        /// <summary>Deletes the object.</summary>
        public void Delete();
    }
}
