using System;
using FHTW.Swen1.Swamp.Base;



namespace FHTW.Swen1.Swamp.Repositories
{
    /// <summary>Repository classes implement this interface.</summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface IRepository<T> where T: IAtom
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets an object by its ID.</summary>
        /// <param name="id">ID.</param>
        /// <returns>Returns the object.</returns>
        public T Get(object id);


        /// <summary>Gets all objects of the repository type.</summary>
        /// <returns>Returns a set of objects.</returns>
        public IEnumerable<T> GetAll();


        /// <summary>Refreshes the object.</summary>
        /// <param name="obj">Object.</param>
        public void Refresh(T obj);


        /// <summary>Saves the object.</summary>
        /// <param name="obj">Object.</param>
        public void Save(T obj);


        /// <summary>Deletes the object.</summary>
        /// <param name="obj">Object.</param>
        public void Delete(T obj);
    }
}
