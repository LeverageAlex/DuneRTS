using System;
namespace Server
{
    public class Party
    {
        private string _lobbyCode;
        private int _cpuCount;

        public Party(string lobbyCode, int cpuCount)
        {
            _lobbyCode = lobbyCode;
            _cpuCount = cpuCount;
        }
    }
}
