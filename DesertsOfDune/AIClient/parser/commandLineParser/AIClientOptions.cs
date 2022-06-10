using System;
using CommandLine;

namespace AIClient.parser
{
    /// <summary>
    /// Defines and specify all command line argument options for the ai client
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
    public class AIClientOptions
    {
        /// <summary>
        /// the command line argument option for the server address
        /// </summary>
        [Option('a', "address", Required = true, HelpText = "The IP address of the server, the party is played on and the client want to connect to.")]
        public string Address { get; set; }

        /// <summary>
        /// the command line argument option for port of the server (optional), the client want to connect to
        /// </summary>
        [Option('p', "port", Default = 10191, HelpText = "The port, on which the server will be opened and the client want to connect to")]
        public int Port { get; set; }

        /// <summary>
        /// the command line argument option for the name of the ai
        /// </summary>
        [Option('n', "name", Default = "team008", HelpText = "The name of the ai. It's format should be team###, where ### represents the team number (e. g. 008 for team 8), but other names are also allowed as long as there are longer than 3 characters.")]
        public string Name { get; set; }
    }
}
