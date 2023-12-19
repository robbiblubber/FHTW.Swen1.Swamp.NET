using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class represents a message.</summary>
    public sealed class Message: IItem
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Repository.</summary>
        private static MessageRepository _Repository = new MessageRepository();

        /// <summary>Token buffer.</summary>
        private const string _ID_BUFFER = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        public Message() 
        {
            Random rnd = new Random();
            ID = string.Empty + _ID_BUFFER[rnd.Next(_ID_BUFFER.Length)] + _ID_BUFFER[rnd.Next(_ID_BUFFER.Length)];

            long d = DateTime.Now.Ticks;
            long l = _ID_BUFFER.Length;
            while(d != 0)
            {
                ID += _ID_BUFFER[Convert.ToInt32(d % l)];
                d /= l;
            }

            for(int i = 0; i < 6; i++) { ID += _ID_BUFFER[rnd.Next(_ID_BUFFER.Length)]; }
            ID = ID.Substring(0, 16);
        }


        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="fromBehind">Flag indicating the object is created by the repository.</param>
        internal Message(bool fromBehind)
        {}


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the message ID.</summary>
        public string ID
        {
            get; internal set;
        } = "";


        /// <summary>Gets the message sender.</summary>
        public User Sender
        {
            get; internal set;
        } = new User();


        /// <summary>Gets the message recipient.</summary>
        public User Recipient
        {
            get; internal set;
        } = new User();


        /// <summary>Gets the message title.</summary>
        public string Title
        {
            get; internal set;
        } = "";


        /// <summary>Gets the message body.</summary>
        public string Body
        {
            get; internal set;
        } = "";



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] IItem                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the object ID.</summary>
        string IItem.ID
        {
            get { return ID; }
        }


        /// <summary>Saves the object.</summary>
        public void Save()
        {
            _Repository.Save(this);
        }


        /// <summary>Deletes the object.</summary>
        public void Delete()
        {
            _Repository.Delete(this);
        }
    }
}
