using System;
using GameData.network.messages;
using GameData.network.util.enums;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace GameData.network.controller
{
    public class MessageEmitter
    {
        private readonly WebSocketTypes TYPE;

        public MessageEmitter(WebSocketTypes type)
        {
            this.TYPE = type;
        }

        public bool SendMessage(Message messageToSend)
        {
        }

        public bool SendMessageTo(Message messageToSend, String cliedSessionID)
        {

        }
    }
}
