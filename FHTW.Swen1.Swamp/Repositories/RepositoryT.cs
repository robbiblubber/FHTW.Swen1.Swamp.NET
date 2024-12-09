using System;
using System.Data;
using System.Data.SQLite;



namespace FHTW.Swen1.Swamp.Repositories
{
    /// <summary>This class provides an abstract implementation of a repository.</summary>
    /// <typeparam name="T">Type.</typeparam>
    public abstract class Repository<T>: IRepository<T>
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Database connection.</summary>
        private static IDbConnection? _DbConnection = null;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected members                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Table name.</summary>
        protected string _TableName = string.Empty;

        /// <summary>ID field name.</summary>
        protected string _IdField = string.Empty;

        /// <summary>Field list.</summary>
        protected string _FieldList = string.Empty;
        


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected static properties                                                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the database connection.</summary>
        protected static IDbConnection _Cn
        {
            get 
            { 
                if(_DbConnection == null)
                {
                    _DbConnection = new SQLiteConnection(@"Data Source = C:\home\projects\FHTW.Swen1.Swamp.NET\res\swamp.db; Version = 3;");
                    _DbConnection.Open();
                }

                return _DbConnection; 
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] IRepsitory<T>                                                                                        //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets an object by its ID.</summary>
        /// <param name="id">ID.</param>
        /// <returns>Returns the object.</returns>
        public virtual T Get(object id)
        {
            // TODO: add implementation.
            throw new NotImplementedException();
        }


        /// <summary>Gets all objects of the repository type.</summary>
        /// <returns>Returns a set of objects.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            // TODO: add implementation.
            throw new NotImplementedException();
        }


        /// <summary>Saves the object.</summary>
        /// <param name="obj">Object.</param>
        /// <param name="user">User that performs the operation.</param>
        public virtual void Save(T obj, User user)
        {
            // TODO: add implementation.
            throw new NotImplementedException();
        }


        /// <summary>Deletes the object.</summary>
        /// <param name="obj">Object.</param>
        /// <param name="user">User that performs the operation.</param>
        public virtual void Delete(T obj, User user)
        {
            // TODO: add implementation.
            throw new NotImplementedException();
        }
    }
}
