using System;
using System.Data;
using FHTW.Swen1.Swamp.Model;

using Thread = FHTW.Swen1.Swamp.Model.Thread;



namespace FHTW.Swen1.Swamp.Repositories
{
    /// <summary>This class provides a repository for entry objects.</summary>
    public class EntryRepository: Repository<Entry>, IRepository<Entry>
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        internal EntryRepository()
        {
            _TableName = "ENTRIES";
            _Fields = ["ID", "TEXT", "TIME", "KTHREAD", "OWNER"];
            _Params = [":id", ":txt", ":tim", ":thr", ":own"];
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the entries for a thread.</summary>
        /// <param name="thread">Thread.</param>
        /// <returns>Returns a set of entries that belong to the given thread.</returns>
        public virtual IEnumerable<Entry> GetFor(Thread thread)
        {
            List<Entry> rval = new();

            using(IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = $"SELECT {string.Join(", ", _Fields)} FROM {_TableName} WHERE KTHREAD = :th";
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = ":th";
                p.Value = thread.ID;
                cmd.Parameters.Add(p);

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



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Repository<Thread>                                                                                    //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Refreshes an object from database data.</summary>
        /// <param name="re">Database cursor.</param>
        /// <param name="obj">Object.</param>
        /// <returns>Returns an object.</returns>
        protected override Entry _RefeshObject(IDataReader re, Entry obj)
        {
            obj.Text = re.GetString(1);
            obj.Time = re.GetDateTime(2);
            obj.Thread = Thread.ByID(re.GetInt32(3));
            obj.Owner = re.GetString(4);

            return obj;
        }


        /// <summary>Sets the database parameters.</summary>
        /// <param name="cmd">Command.</param>
        /// <param name="obj">Object.</param>
        protected override void _FillParameters(IDbCommand cmd, Entry obj)
        {
            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":txt";
            p.Value = obj.Text;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":tim";
            p.Value = obj.Time;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":thr";
            p.Value = obj.Thread?.ID ?? -1;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":own";
            p.Value = obj.Owner;
            cmd.Parameters.Add(p);
        }
    }
}
