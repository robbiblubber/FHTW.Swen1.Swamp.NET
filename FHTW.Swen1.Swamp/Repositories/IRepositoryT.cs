using System;



namespace FHTW.Swen1.Swamp.Repositories
{
    /// <summary>Repository classes implement this interface.</summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface IRepository<T>
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


        /// <summary>Saves the object.</summary>
        /// <param name="obj">Object.</param>
        /// <param name="user">User that performs the operation.</param>
        public void Save(T obj, User user);


        /// <summary>Deletes the object.</summary>
        /// <param name="obj">Object.</param>
        /// <param name="user">User that performs the operation.</param>
        public void Delete(T obj, User user);
    }
}
