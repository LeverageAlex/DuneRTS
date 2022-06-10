using System;
using System.Net;
using System.Text.RegularExpressions;
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
            // check, whether the ip address has the correct format
            bool correcFormat = Regex.IsMatch(address, "\\d{1,3}.\\d{1,3}.\\d{1,3}.\\d{1,3}");

            if (correcFormat)
            {
                // check, whether it is syntactically correct:
                IPAddress ipAddress = null;
                return IPAddress.TryParse(address, out ipAddress);
            }

            return false;
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
