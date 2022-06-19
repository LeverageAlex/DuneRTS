using System;
using System.Runtime.ConstrainedExecution;

namespace GameData.Configuration
{
    /// <summary>
    /// Configuration of the server, which holds all constants and information needed by the server
    /// </summary>
    public class ServerConfiguration
    {
        public static int DEFAULT_PORT { get; } = 10191;
        public static string DEFAULT_SERVER_ADDRESS { get; } = "127.0.0.1";

        public int Port { get; set; }
        public string FilePathMatchConfiguration { get; set; }
        public string FilePathScenarioConfiguration { get; set; }

        public ServerConfiguration()
        {
        }
    }
}
