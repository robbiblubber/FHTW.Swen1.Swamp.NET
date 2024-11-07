using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class represents a HTTP header.</summary>
    public class HttpHeader
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="header">Raw header string.</param>
        public HttpHeader(string header)
        {
            Name = Value = string.Empty;

            try
            {
                int n = header.IndexOf(':');
                Name = header.Substring(0, n).Trim();
                Value = header.Substring(n + 1).Trim();
            }
            catch(Exception) {}
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the header name.</summary>
        public string Name
        {
            get; protected set;
        }


        /// <summary>Gets the header value.</summary>
        public string Value
        {
            get; protected set;
        }
    }
}
