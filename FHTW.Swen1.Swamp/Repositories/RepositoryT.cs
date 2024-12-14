using System;
using System.Data;
using System.Data.SQLite;
using FHTW.Swen1.Swamp.Base;



namespace FHTW.Swen1.Swamp.Repositories
{
    /// <summary>This class provides an abstract implementation of a repository.</summary>
    /// <typeparam name="T">Type.</typeparam>
    public abstract class Repository<T>: IRepository<T> where T: IAtom, new()
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

        /// <summary>Field names.</summary>
        protected string[] _Fields = Array.Empty<string>();

        /// <summary>Parameter names.</summary>
        protected string[] _Params = Array.Empty<string>();
        


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
        // protected methods                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates an object from database data.</summary>
        /// <param name="re">Database cursor.</param>
        /// <returns>Returns an object.</returns>
        protected virtual T _CreateObject(IDataReader re)
        {
            T rval = new();
            ((__IAtom) rval).__InternalID = re.GetString(0);
            return _RefeshObject(re, rval);
        }


        /// <summary>Refreshes an object from database data.</summary>
        /// <param name="re">Database cursor.</param>
        /// <param name="obj">Object.</param>
        /// <returns>Returns an object.</returns>
        protected abstract T _RefeshObject(IDataReader re, T obj);


        /// <summary>Sets the database parameters.</summary>
        /// <param name="cmd">Command.</param>
        /// <param name="obj">Object.</param>
        protected abstract void _FillParameters(IDbCommand cmd, T obj);



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] IRepsitory<T>                                                                                        //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets an object by its ID.</summary>
        /// <param name="id">ID.</param>
        /// <returns>Returns the object.</returns>
        public virtual T Get(object id)
        {
            using(IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = $"SELECT {string.Join(", ", _Fields)} FROM {_TableName} WHERE {_Fields[0]} = :id";
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = ":id";
                p.Value = id;
                cmd.Parameters.Add(p);

                using(IDataReader re = cmd.ExecuteReader())
                {
                    if(re.Read())
                    {
                        return _CreateObject(re);
                    }
                }
            }

            throw new DataException("No such object.");
        }


        /// <summary>Gets all objects of the repository type.</summary>
        /// <returns>Returns a set of objects.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            List<T> rval = new();

            using(IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = $"SELECT {string.Join(", ", _Fields)} FROM {_TableName}";

                using(IDataReader re = cmd.ExecuteReader())
                {
                    while(re.Read())
                    {
                        rval.Add(_CreateObject(re));
                    }
                }
            }

            return rval;
        }


        /// <summary>Refreshes the object.</summary>
        /// <param name="obj">Object.</param>
        public virtual void Refresh(T obj)
        {
            using(IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = $"SELECT {string.Join(", ", _Fields)} FROM {_TableName} WHERE {_Fields[0]} = :id";
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = ":id";
                p.Value = ((__IAtom) obj).__InternalID;
                cmd.Parameters.Add(p);

                using(IDataReader re = cmd.ExecuteReader())
                {
                    if(re.Read())
                    {
                        _RefeshObject(re, obj);
                    }
                }
            }
        }


        /// <summary>Saves the object.</summary>
        /// <param name="obj">Object.</param>
        public virtual void Save(T obj)
        {
            if(obj.__InternalID is null)
            {
                using(IDbCommand cmd = _Cn.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO {_TableName} ({string.Join(", ", _Fields.Skip(1))}) VALUES ({string.Join(", ", _Params.Skip(1))})";
                    _FillParameters(cmd, obj);
                    cmd.ExecuteNonQuery();
                }

                using(IDbCommand cmd = _Cn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT last_insert_rowid()";
                    obj.__InternalID = (int) cmd.ExecuteScalar()!;
                }
            }
            else
            {
                using(IDbCommand cmd = _Cn.CreateCommand())
                {
                    cmd.CommandText = $"UPDATE {_TableName} SET ";
                    for(int i = 1; i < _Fields.Length; i++)
                    {
                        if(i > 1) { cmd.CommandText += ", "; }
                        cmd.CommandText += (_Fields[i] + " = " + _Params[i]);
                    }
                    cmd.CommandText += $" WHERE {_Fields[0]} = :id";
                    
                    _FillParameters(cmd, obj);
                    IDataParameter p = cmd.CreateParameter();
                    p.ParameterName = ":id";
                    p.Value = obj.__InternalID;
                    cmd.Parameters.Add(p);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>Deletes the object.</summary>
        /// <param name="obj">Object.</param>
        /// <param name="user">User that performs the operation.</param>
        public virtual void Delete(T obj)
        {
            using(IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = $"DELETE FROM {_TableName} WHERE {_Fields[0]} = :id";
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = ":id";
                p.Value = obj.__InternalID;
                cmd.Parameters.Add(p);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
