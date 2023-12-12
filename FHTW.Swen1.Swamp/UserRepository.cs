using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class implements the user repository.</summary>
    public sealed class UserRepository: Repository<User>, IRepository<User>
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        internal UserRepository()
        {
            _Table = "USERS";
            _Fields = "ID, NAME";
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Hashes a password.</summary>
        /// <param name="password">Password.</param>
        /// <returns>Hash value.</returns>
        private string _HashPassword(string password)
        {
            return _ToHex(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
        }


        /// <summary>Converts a byte array to a hexadecimal string.</summary>
        /// <param name="bytes">Byte array.</param>
        /// <returns>Hexadecimal representation of the array.</returns>
        private static string _ToHex(byte[] bytes)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for(int i = 0; i < bytes.Length; i++) result.Append(bytes[i].ToString("x2"));
            return result.ToString();
        }




        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Cretaes a new user object.</summary>
        /// <param name="id">ID.</param>
        /// <param name="name">Name.</param>
        /// <param name="password">Password.</param>
        /// <returns>Returns the newly created user object.</returns>
        public User Create(string id, string name, string password)
        {
            IDbCommand cmd = _Cn.CreateCommand();

            cmd.CommandText = $"INSERT INTO {_Table} (ID, NAME, PASSWORD) VALUES (:id, :n, :p)";
            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":id";
            p.Value = id;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":n";
            p.Value = name;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":p";
            p.Value = _HashPassword(password);
            cmd.Parameters.Add(p);

            cmd.ExecuteNonQuery();
            cmd.Dispose();

            return new User() { ID = id, Name = name };
        }


        /// <summary>Changes a user password.</summary>
        /// <param name="obj">User object.</param>
        /// <param name="password">New password.</param>
        public void ChangePassword(User obj, string password)
        {
            IDbCommand cmd = _Cn.CreateCommand();

            cmd.CommandText = $"UPDATE {_Table} SET PASSWORD = :p WHERE ID = :id";
            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":id";
            p.Value = obj.ID;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":p";
            p.Value = _HashPassword(password);
            cmd.Parameters.Add(p);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }


        /// <summary>Verifies a user password.</summary>
        /// <param name="obj">Object.</param>
        /// <param name="password">Password.</param>
        /// <returns>Returns TRUE if the password has been successfully verified, otherwise returns FALSE.</returns>
        public bool VerifyPassword(User obj, string password)
        {
            using(IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = $"SELECT COUNT(*) FROM {_Table} WHERE ID = :id AND PASSWORD = :p";
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = ":id";
                p.Value = obj.ID;
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = ":p";
                p.Value = _HashPassword(password);
                cmd.Parameters.Add(p);

                return (Convert.ToInt32(cmd.ExecuteScalar()) == 1);
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Repository<User>                                                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Sets the data for an object instance.</summary>
        /// <param name="obj">Object.</param>
        /// <param name="re">Database cursor.</param>
        protected override void _Fill(User obj, IDataReader re)
        {
            obj.ID = re.GetString(0);
            obj.Name = re.GetString(1);
        }


        /// <summary>Saves an object.</summary>
        /// <param name="obj">Object.</param>
        public override void Save(User obj)
        {
            IDbCommand cmd = _Cn.CreateCommand();
            cmd.CommandText = $"UPDATE {_Table} SET NAME = :n WHERE ID = :id";
            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":n" ;
            p.Value = obj.Name;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":id";
            p.Value = obj.ID;
            cmd.Parameters.Add(p);
            
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}
