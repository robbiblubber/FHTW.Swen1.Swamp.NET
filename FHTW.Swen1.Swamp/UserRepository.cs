using System;
using System.Data;

using FHTW.Swen1.Swamp.Repositories;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class provides a repository for user objects.</summary>
    public sealed class UserRepository: Repository<User>, IRepository<User>
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        internal UserRepository()
        {
            _TableName = "USERS";
            _Fields = ["USERNAME", "NAME", "EMAIL", "HADMIN"];
            _Params = [":id", ":n", ":m", ":a"];
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected methods                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Refreshes an object from database data.</summary>
        /// <param name="re">Database cursor.</param>
        /// <param name="obj">Object.</param>
        /// <returns>Returns an object.</returns>
        protected override User _RefeshObject(IDataReader re, User obj)
        {
            obj.FullName = re.GetString(1);
            obj.EMail = re.GetString(2);
            obj._IsAdmin = re.GetBoolean(3);

            return obj;
        }


        /// <summary>Sets the database parameters.</summary>
        /// <param name="cmd">Command.</param>
        /// <param name="obj">Object.</param>
        protected override void _FillParameters(IDbCommand cmd, User obj)
        {
            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":n";
            p.Value = obj.FullName;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":m";
            p.Value = obj.EMail;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":a";
            p.Value = obj.IsAdmin;
            cmd.Parameters.Add(p);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Repository<User>                                                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Saves the object.</summary>
        /// <param name="obj">Object.</param>
        public override void Save(User obj)
        {
            if(((__IAtom) obj).__InternalID is null)
            {
                using(IDbCommand cmd = _Cn.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO {_TableName} ({string.Join(", ", _Fields)}) VALUES ({string.Join(", ", _Params)})";
                    
                    _FillParameters(cmd, obj);
                    IDataParameter p = cmd.CreateParameter();
                    p.ParameterName = _Params[0];
                    p.Value = obj.UserName;
                    cmd.Parameters.Add(p);
                    
                    cmd.ExecuteNonQuery();
                }

                ((__IAtom) obj).__InternalID = obj.UserName;
            }
            else { base.Save(obj); }
        }
    }
}
