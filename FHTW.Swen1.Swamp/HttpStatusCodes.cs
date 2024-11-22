using System;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This enumeration defines HTTP status codes that are used by
    ///          the <see cref="HttpSvr"/> implementation.</summary>
    public static class HttpStatusCode
    {
        /// <summary>Status code OK.</summary>
        public const int OK = 200;

        /// <summary>Status code BAD REQUEST.</summary>
        public const int BAD_REQUEST = 400;

        /// <summary>Status code UNAUTHORIZED.</summary>
        public const int UNAUTHORIZED = 401;

        /// <summary>Status code NOT FOUND.</summary>
        public const int NOT_FOUND = 404;
    }
}
