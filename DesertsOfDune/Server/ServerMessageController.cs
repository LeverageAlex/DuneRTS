using System;
using GameData.network.controller;
using GameData.network.messages;

namespace Server
{
    public class ServerMessageController
    {
        public ServerMessageController()
        {
            
        }

        public void OnCreate(CreateMessage msg)
        {
            //Client sendet CreateMsg wenn er eine neue Partie erstellen möchte

            //string lobbyCode
            //int cpuCount
        }

        public void OnJoin(JoinMessage msg)
        {
            //Server erwartet Registrierung von Client
            //wird auch beim reconnect verwendet

            //string clientName
            //string connectionCode
            //bool active
        }

        public void DoCreateResponse(CreateMessage msg)
        {
            //Server sendet Bestätigung oder Fehlernachricht

            //string lobbyCode
            //int cpuCount
            
        }

        public void DoJoinAccepted(JoinAcceptedMessage msg)
        {
            //Server bestätigt erfolgreiche Registrierung mit

            //string clientSecret
        }
    }
}
