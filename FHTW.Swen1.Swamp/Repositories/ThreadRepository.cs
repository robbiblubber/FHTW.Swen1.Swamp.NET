using System;
using System.Data;
using FHTW.Swen1.Swamp.Base;

using FHTW.Swen1.Swamp.Security;

using Thread = FHTW.Swen1.Swamp.Model.Thread;



namespace FHTW.Swen1.Swamp.Repositories
{
    /// <summary>This class provides a repository for thread objects.</summary>
    public class ThreadRepository: Repository<Thread>, IRepository<Thread>
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        internal ThreadRepository()
        {
            _TableName = "THREADS";
            _Fields = ["ID", "TITLE", "TIME", "OWNER"];
            _Params = [":id", ":tit", ":tim", ":own"];
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Repository<Thread>                                                                                    //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Refreshes an object from database data.</summary>
        /// <param name="re">Database cursor.</param>
        /// <param name="obj">Object.</param>
        /// <returns>Returns an object.</returns>
        protected override Thread _RefeshObject(IDataReader re, Thread obj)
        {
            obj.Title = re.GetString(1);
            obj.Time = re.GetDateTime(2);
            obj.Owner = re.GetString(3);

            return obj;
        }


        /// <summary>Sets the database parameters.</summary>
        /// <param name="cmd">Command.</param>
        /// <param name="obj">Object.</param>
        protected override void _FillParameters(IDbCommand cmd, Thread obj)
        {
            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":tit";
            p.Value = obj.Title;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":tim";
            p.Value = obj.Time;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":own";
            p.Value = obj.Owner;
            cmd.Parameters.Add(p);
        }
    }
}
