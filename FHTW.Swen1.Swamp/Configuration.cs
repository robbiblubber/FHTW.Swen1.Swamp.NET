using System;
using System.Text.Json;



namespace FHTW.Swen1.Swamp
{
    /// <summary>This class provides configuration data.</summary>
    public sealed class Configuration
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Instance.</summary>
        private static Configuration? _Instance = null;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static properties                                                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the configuration instance.</summary>
        public static Configuration Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(@"C:\home\projects\FHTW.SWEN1.Swamp.NET\swamp.config"));
                }

                return _Instance ?? new Configuration();
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the database path.</summary>
        public string DatabasePath
        {
            get; set;
        } = "";
    }
}
