using System;
using Serilog;

namespace GameData.validation
{
    /// <summary>
    /// Validtor for the ai client configuration entries respectivily the command line arguments from the ai client
    /// </summary>
    public class AIClientConfigurationValidator : ConfigurationValidator
    {
        public AIClientConfigurationValidator()
        {
        }

        /// <summary>
        /// checks, whether a given server IPv4 address is syntactically correct
        /// </summary>
        /// <param name="address">the address, which need to be checked</param>
        /// <returns>true, if the address is a valid IPv4 address</returns>
        /// TODO: try to connect to check, whether this address is semantically correct too
        public bool IsServerAddressValid(string address)
        {
            
        }

        /// <summary>
        /// checks, whether a given name is correct in the meaning the standard, so is longer than 3 characters
        /// </summary>
        /// <param name="name">the name of the, which will be checked</param>
        /// <returns>true, if the name is valid</returns>
        public bool IsNameValid(string name)
        {
            return name.Length > 3;
        }
    }
}
