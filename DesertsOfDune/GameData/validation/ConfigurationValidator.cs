using System;
using Serilog;

namespace GameData.validation
{
    /// <summary>
    /// Base class for all configuartion validator, which also implement all common validator methods
    /// </summary>
    public abstract class ConfigurationValidator
    {
        protected ConfigurationValidator()
        {
        }

        /// <summary>
        /// checks, whether a given port is valid in terms of the command standard.
        /// This means, the port must be a number between 1024 and 65535.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool IsPortValid(int port)
        {
            if (port < 0)
            {
                Log.Error("The given port " + port + " is negative. A port number must be always positive!");
                return false;
            } else if (port < 1024)
            {
                Log.Error("The given port " + port + " is below 1024. These ports are the well-known ports and cannot be used, because they need superuser privileges!");
                return false;
            } else if (port > 65535)
            {
                Log.Error("The given port " + port + " is greate than 65535. This is the highest possible port number, so choose a port number below!");
                return false;
            }
            // port is valid, so return true
            return true;
        }
    }
}
