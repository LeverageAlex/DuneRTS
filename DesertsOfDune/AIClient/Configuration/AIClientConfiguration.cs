using System;
namespace AIClient.Configuration
{
    /// <summary>
    /// Stores all configuration information for the ai client
    /// </summary>
    /// <remarks>
    /// This means, that the following information are stored in this class and can be used for configuring (e.g. the network module)
    /// <list type="bullet">
    /// <item>server address</item>
    /// <item>server port</item>
    /// <item>name of the ai</item>
    /// </list>
    /// </remarks>
    public class AIClientConfiguration
    {
        public static int DEFAULT_PORT { get; } = 10191;
        public static string DEFAULT_SERVER_ADDRESS { get; } = "127.0.0.1";
        public static string DEFAULT_NAME { get; } = "ki008";

        public string Address { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }

        public AIClientConfiguration()
        {
        }
    }
}
