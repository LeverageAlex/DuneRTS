using System;
using Serilog;

namespace GameData.validation
{
    /// <summary>
    /// Validtor for the server configuration entries respectivily the command line arguments from the server
    /// </summary>
    public class ServerConfigurationValidator : ConfigurationValidator
    {
        public ServerConfigurationValidator()
        {
        }
        // TODO: try to load the files and check, whether they really exist at the filepath

        /// <summary>
        /// checks, whether the given filepath is valid as a absolut or relative filepath to the match configuration file
        /// </summary>
        /// <param name="filepath">the path to the match configuration file</param>
        public bool IsMatchConfigFilePathValid(string filepath)
        {
            // use the a method, that checks, whether the filepath to the match config is a valid uri
            bool validFilePathSyntax = Uri.IsWellFormedUriString(filepath, UriKind.RelativeOrAbsolute);
            if (validFilePathSyntax)
            {
                // the filepath is correct uri syntax, so check, whether the file has the correct file ending
                if (filepath.EndsWith(".party.json"))
                {
                    Log.Debug("The given filepath to the match configuration file is valid.");
                    return true;
                }
                else
                {
                    Log.Error("The given file (path) do not contain the correct file or the file do not have the correct file ending (need to end with .party.json)");
                }

            }
            else
            {
                Log.Error("The filepath is not wellformed, so no valid absolute or relative file path!");
            }

            return false;
        }

        /// <summary>
        /// checks, whether the given filepath is valid as a absolut or relative filepath to the scenario configuration file
        /// </summary>
        /// <param name="filepath">the path to the scenario configuration file</param>
        public bool IsScenarioConfigFilePathValid(string filepath)
        {
            // use the a method, that checks, whether the filepath to the match config is a valid uri
            bool validFilePathSyntax = Uri.IsWellFormedUriString(filepath, UriKind.RelativeOrAbsolute);
            if (validFilePathSyntax)
            {
                // the filepath is correct uri syntax, so check, whether the file has the correct file ending
                if (filepath.EndsWith(".scenario.json"))
                {
                    Log.Debug("The given filepath to the scenario configuration file is valid.");
                    return true;
                }
                else
                {
                    Log.Error("The given file (path) do not contain the correct file or the file do not have the correct file ending (need to end with .scenario.json)");
                }

            }
            else
            {
                Log.Error("The filepath is not wellformed, so no valid absolute or relative file path!");
            }

            return false;
        }
    }
}
