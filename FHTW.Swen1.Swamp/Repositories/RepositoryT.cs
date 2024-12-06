using System;
using System.Data;
using System.Data.SQLite;

namespace FHTW.Swen1.Swamp.Repositories
{
    public abstract class Repository<T>: IRepository<T>
    {
        private static IDbConnection? _DbConnection = null;

        protected string _TABLE_NAME = string.Empty;



        public virtual void Delete(T obj)
        {
            
        }

        public virtual T Get(object id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual void Save(T obj)
        {
            throw new NotImplementedException();
        }


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
    }
}
