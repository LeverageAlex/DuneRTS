using System;
using System.Collections.Generic;
using System.Transactions;
using CommandLine;
using CommandLine.Text;
using Serilog;

namespace GameData.network.util.parser
{
    /// <summary>
    /// Base class for a command line arguments parser
    /// </summary>
    abstract public class ACommandLineParser<T>
    {
        private Parser parser;
        private readonly T typeOfParser;

        /// <summary>
        /// creates a new command line parser and trigger the initialization of the parser
        /// </summary>
        protected ACommandLineParser()
        {
        }

        /// <summary>
        /// initializes the parser, so create a default parser
        /// </summary>
        protected void InitializeParser()
        {
            this.parser = new Parser(config => config.AutoHelp = true);
        }

        /// <summary>
        /// parsing the arguments 
        /// </summary>
        /// <param name="args"></param>
        /// <returns>true, if the command line arguments were parsed completely or the help / version was requested</returns>
        public bool ParseCommandLineArguments(string[] args)
        {
            ParserResult<T> result = this.parser.ParseArguments<T>(args);

            if (result.Tag == ParserResultType.NotParsed)
            {
                return HandleErrors(result.Errors, result);
            }
            else
            {
                return HandleParsedArguments(result.Value);
            }
        }

        /// <summary>
        /// handle errors if the parsing of the arguments didn't work. Possible reasons are:
        /// <list type="bullet">
        /// <item>help was requested (then print it to console)</item>
        /// <item>version was requested (then print it to console)</item>
        /// <item>an error occured, e.g. missing argument (then print type to console)</item>
        /// </list>
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="result"></param>
        /// <returns>true, if the help was requested and false, if there were any errors</returns>
        private bool HandleErrors(IEnumerable<Error> errors, ParserResult<T> result)
        {
            if (errors.IsHelp())
            {
                Log.Information(HelpText.AutoBuild(result, _ => _, _ => _));
                return true;
            }
            else
            {
                // TODO: catch error message and provide more meaningful messages
                foreach (Error err in errors)
                {
                    Log.Error(err.ToString());
                }
            }
            return false;
        }

        abstract protected bool HandleParsedArguments(T options);
    }
}