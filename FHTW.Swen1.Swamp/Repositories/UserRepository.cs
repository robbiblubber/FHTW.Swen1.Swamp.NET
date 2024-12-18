using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

using FHTW.Swen1.Swamp.Base;
using FHTW.Swen1.Swamp.Security;



namespace FHTW.Swen1.Swamp.Repositories
{
    /// <summary>This class provides a repository for user objects.</summary>
    public sealed class UserRepository : Repository<User>, IRepository<User>
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
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Verifies the user password.</summary>
        /// <param name="user">User.</param>
        /// <param name="password">Password.</param>
        /// <returns>Returns TRUE if the password is valid, otherwise returns FALSE.</returns>
        public bool VerifyPassword(User user, string password)
        {
            using (IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = $"SELECT COUNT(*) FROM {_TableName} WHERE USERNAME = :un AND PASSWD = :pw";

                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = ":un";
                p.Value = user.UserName;
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = ":pw";
                p.Value = _GetPasswordHash(user, password);
                cmd.Parameters.Add(p);

                return (Convert.ToInt32(cmd.ExecuteScalar()) == 1);
            }
        }


        /// <summary>Sets the user password.</summary>
        /// <param name="user">User.</param>
        /// <param name="password">Password.</param>
        public void SetPassword(User user, string password) 
        {
            using (IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = $"UPDATE {_TableName} SET PASSWD = :pw WHERE USERNAME = :un";

                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = ":pw";
                p.Value = _GetPasswordHash(user, password);
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = ":un";
                p.Value = user.UserName;
                cmd.Parameters.Add(p);

                cmd.ExecuteNonQuery();
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private methods                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the user password hash.</summary>
        /// <param name="user">User object.</param>
        /// <param name="password">Password.</param>
        /// <returns>Gets the password hash for the user.</returns>
        private string _GetPasswordHash(User user, string password)
        {
            using(SHA256 sha256Hash = SHA256.Create())
            {
                byte[] buf = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(user.UserName + password));

                StringBuilder rval = new StringBuilder();
                foreach(byte b in buf) { rval.Append(b.ToString("x2")); }
                
                return rval.ToString();
            }
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

        /// <summary>Creates an object from database data.</summary>
        /// <param name="re">Database cursor.</param>
        /// <returns>Returns an object.</returns>
        protected override User _CreateObject(IDataReader re)
        {
            User rval = new();
            ((__IAtom) rval).__InternalID = re.GetString(0);
            return _RefeshObject(re, rval);
        }


        /// <summary>Saves the object.</summary>
        /// <param name="obj">Object.</param>
        public override void Save(User obj)
        {
            if (((__IAtom)obj).__InternalID is null)
            {
                using (IDbCommand cmd = _Cn.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO {_TableName} ({string.Join(", ", _Fields)}) VALUES ({string.Join(", ", _Params)})";

                    _FillParameters(cmd, obj);
                    IDataParameter p = cmd.CreateParameter();
                    p.ParameterName = _Params[0];
                    p.Value = obj.UserName;
                    cmd.Parameters.Add(p);

                    cmd.ExecuteNonQuery();
                }

                ((__IAtom)obj).__InternalID = obj.UserName;
            }
            else { base.Save(obj); }
        }
    }
}
