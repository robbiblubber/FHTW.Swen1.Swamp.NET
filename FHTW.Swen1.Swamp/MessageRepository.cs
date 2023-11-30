using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class provides a message repository.</summary>
    public sealed class MessageRepository: Repository<Message>, IRepository<Message>
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        public MessageRepository()
        {
            _Table = "MESSAGES";
            _Fields = "ID, KSENDER, KRECIPIENT, TITLE, BODY";
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [override] Repository<User>                                                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Sets the data for an object instance.</summary>
        /// <param name="obj">Object.</param>
        /// <param name="re">Database cursor.</param>
        protected override void _Fill(Message obj, IDataReader re)
        {
            obj.ID = re.GetString(0);
            obj.Sender = User.ByID(re.GetString(1));
            obj.Recipient = User.ByID(re.GetString(2));
            obj.Title = re.GetString(3);
            obj.Body = re.GetString(4);
        }


        /// <summary>Saves an object.</summary>
        /// <param name="obj">Object.</param>
        public override void Save(Message obj)
        {
            IDbCommand cmd = _Cn.CreateCommand();
            cmd.CommandText = $"INSERT INTO {_Table} ({_Fields}) VALUES (:id, :s, :r, :t, :b)";
            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":id";
            p.Value = obj.ID;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":s";
            p.Value = obj.Sender.ID;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":r";
            p.Value = obj.Recipient.ID;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":t";
            p.Value = obj.Title;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":b";
            p.Value = obj.Body;
            cmd.Parameters.Add(p);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}
