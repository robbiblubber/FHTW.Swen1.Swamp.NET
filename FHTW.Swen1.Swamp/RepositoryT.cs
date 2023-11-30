using System;
using System.Data;
using System.Data.SQLite;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class provides a repository base implementation.</summary>
    public abstract class Repository<T>: IRepository<T> where T: class, new()
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected static members                                                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Database connection.</summary>
        protected readonly static IDbConnection _Cn = _Connect();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected members                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Database table name.</summary>
        protected string _Table = "";

        /// <summary>Table fields list.</summary>
        protected string _Fields = "";



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected methods                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Sets the data for an object instance.</summary>
        /// <param name="obj">Object.</param>
        /// <param name="re">Database cursor.</param>
        protected abstract void _Fill(T obj, IDataReader re);



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates and returns a valid database connection.</summary>
        /// <returns>Database connection.</returns>
        private static IDbConnection _Connect()
        {
            IDbConnection rval = new SQLiteConnection($"Data Source = {Configuration.Instance.DatabasePath}; Version = 3;");
            rval.Open();

            return rval;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] IRepository<T>                                                                                       //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the object by its ID.</summary>
        /// <param name="id">ID.</param>
        /// <returns>Returns the retrieved object or NULL if there is no object with the given ID..</returns>
        public virtual T? Get(string id)
        {
            T? rval = null;

            IDbCommand cmd = _Cn.CreateCommand();
            cmd.CommandText = $"SELECT {_Fields} FROM {_Table} WHERE ID = :id";
            IDataReader re = cmd.ExecuteReader();

            if(re.Read())
            {
                rval = new();
                _Fill(rval, re);
            }

            re.Close();
            re.Dispose(); cmd.Dispose();

            return rval;
        }


        /// <summary>Gets all objects.</summary>
        /// <returns>List of objects.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            List<T> rval = new();

            IDbCommand cmd = _Cn.CreateCommand();
            cmd.CommandText = $"SELECT {_Fields} FROM {_Table}";
            IDataReader re = cmd.ExecuteReader();

            while(re.Read())
            {
                T v = new();
                _Fill(v, re);
                rval.Add(v);
            }

            re.Close();
            re.Dispose(); cmd.Dispose();

            return rval;
        }


        /// <summary>Delets an object.</summary>
        /// <param name="obj">Object.</param>
        public virtual void Delete(T obj)
        {
            IDbCommand cmd = _Cn.CreateCommand();
            cmd.CommandText = $"SELECT {_Fields} FROM {_Table}";
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }


        /// <summary>Saves an object.</summary>
        /// <param name="obj">Object.</param>
        public abstract void Save(T obj);
    }
}
