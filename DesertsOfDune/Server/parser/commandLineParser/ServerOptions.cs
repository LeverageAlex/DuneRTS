using System;
using CommandLine;

namespace GameData.parser
{
    /// <summary>
    /// This class defines and specify all command line argument options for the server
    /// </summary>
    /// <remarks>
    /// Therefore this class use the CommandLine-package stores variables for every possible argument.
    /// Furthermore
    /// <list type="bullet">
    /// <item>the parameter name in the cli</item>
    /// <item>the alias</item>
    /// <item>whether the argument is reqiured</item>
    /// <item>a help text</item>
    /// is defined for every argument
    /// </list>
    /// The arguments, which are defined in this class, can be found in the Version 0.3 of the "Standidisierungsdokument
    /// </remarks>
    public class ServerOptions
    {
        /// <summary>
        /// the command line argument option for the path to the party configuration file
        /// </summary>
        [Option('m', "config-match", Required = true, HelpText = "The absolute or relative path to the party / match configuration file. ")]
        public string ConfigurationFileMatch {get; set;}


        /// <summary>
        /// the command line argument option for the path to the scenario configuration file
        /// </summary>
        [Option('s', "config-scenario", Required = true, HelpText = "The absolute or relative path to the scenario configuration file. ")]
        public string ConfigurationFileScenario { get; set; }


        /// <summary>
        /// the command line argument option for port of the server (optional)
        /// </summary>
        [Option('p', "port", Default = 10191, HelpText = "The port, on which the server will open the websocket server")]
        public int Port { get; set; }
    }
}
