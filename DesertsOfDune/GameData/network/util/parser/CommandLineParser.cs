using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Server.commandLineParser
{
    /// <summary>
    /// This class is responsible for parsing commandline input.
    /// </summary>
    public static class CommandLineParser
    {
        /// <summary>
        /// This method prints all commandline attributes and their meaning to the commandLine.
        /// </summary>
        private static void PrintHelp()
        {
            Console.WriteLine("Parameter: -- config-match expects a absolut or relativ path to file <Name>.party.json (alias: -m <path>");
            Console.WriteLine("Parameter: -- config-scenario expects a absolut or relativ path to file <Name>.scenario.json (alias: -m <path>)");
            Console.WriteLine("Parameter: -- x expects <KEY> <Value> paris for settings");
            Console.WriteLine("Parameter: -- help prints information about all possible parameters");
            Console.WriteLine("Parameter: -- port <portNumber> expects a port number or sets it by default to 10191");
        }

        /// <summary>
        /// This method parses the argument and triggers its handling
        /// </summary>
        /// <param name="parameter">the parameter that should be parsed and handeld</param>
        public static void ParseArgument(string parameter)
        {
            string pattern = "-- .*";
            Regex regex = new Regex(pattern);
            MatchCollection match = regex.Matches(parameter);
            Console.WriteLine("count matches: " + match.Count);

            if (match.Count == 1)
            {
                parseComandLineArgument(match);
            } else
            {
                string p = "-[a-z].*";
                Regex r = new Regex(p);
                MatchCollection m = r.Matches(parameter);
                if (m.Count == 1)
                {
                    ParseCommandLineAlias(m);
                }
            }
        }

        /// <summary>
        /// This method parses a commandLine argument
        /// </summary>
        /// <param name="match">the match for the commandline argument</param>
        private static void parseComandLineArgument(MatchCollection match)
        {
            // commandline arg was Parameter
            string[] arguments = match[0].Value.Split(' ');
            Console.WriteLine("the match: " + arguments[1]);
            switch (arguments[1])
            {
                case "config-match":
                    Console.WriteLine("case config match with param: " + arguments[2]);
                    break;
                case "config-scenario":
                    Console.WriteLine("case config scenario with param: " + arguments[2]);
                    break;
                case "x":
                    Console.WriteLine("case x with key: " + arguments[2] + " and value: " + arguments[3]);
                    break;
                case "help":
                    PrintHelp();
                    break;
                case "port":
                    if (arguments.Length < 3)
                    {
                        Console.WriteLine("use default portnummer 10191");
                    }
                    else
                    {
                        Console.WriteLine("case port with param: " + arguments[2]);
                    }
                    break;
                case "adress":
                    Console.WriteLine("case adress");
                    break;
                case "name":
                    Console.WriteLine("case name");
                    break;
                default:
                    Console.WriteLine("no case catched.");
                    break;
            }
        }

        /// <summary>
        /// This method parses a commandline alias
        /// </summary>
        /// <param name="m">the match of the commandline alias</param>
        private static void ParseCommandLineAlias(MatchCollection m)
        {
            // commandline argument was Alias
            string[] args = m[0].Value.Split(' ');
            switch (args[0])
            {
                case "-m":
                    Console.WriteLine("case alias -m with parameter: " + args[1]);
                    break;
                case "-s":
                    Console.WriteLine("case alias -s with parameter: " + args[1]);
                    break;
                case "-h":
                    PrintHelp();
                    break;
                case "-p":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("use default portnummer 10191");
                    }
                    else
                    {
                        Console.WriteLine("case port with param: " + args[1]);
                    }
                    break;
                case "-a":
                    Console.WriteLine("case alias -a");
                    break;
                case "-n":
                    Console.WriteLine("case alias -n");
                    break;
            }
        }
    }
}
