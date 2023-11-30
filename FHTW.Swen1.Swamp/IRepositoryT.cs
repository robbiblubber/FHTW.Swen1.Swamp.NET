using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>Repositories implement this interface.</summary>
    public interface IRepository<T>
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // methods                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the object by its ID.</summary>
        /// <param name="id">ID.</param>
        /// <returns>Object.</returns>
        public T? Get(string id);


        /// <summary>Gets all objects.</summary>
        /// <returns>List of objects.</returns>
        public IEnumerable<T> GetAll();


        /// <summary>Delets an object.</summary>
        /// <param name="obj">Object.</param>
        public void Delete(T obj);


        /// <summary>Saves an object.</summary>
        /// <param name="obj">Object.</param>
        public void Save(T obj);
    }
}
