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
