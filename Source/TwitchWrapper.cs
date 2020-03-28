using Colonystats.Twitch;
using Colonystats.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using Verse;

namespace Colonystats
{
    public static class TwitchWrapper
    {
        public static TwitchClient Client { get; private set; }

        public static readonly List<ITwitchTranslator> TRANSLATORS = new List<ITwitchTranslator>();

        public static void StartAsync(bool bypass)
        {
            Initialize(new ConnectionCredentials(ColonyStatsSettings.bot_username, ColonyStatsSettings.oauth_token, "wss://irc-ws.chat.twitch.tv:443", true), bypass);
        }

        public static void Initialize(ConnectionCredentials credentials, bool bypass)
        {
            ResetClient();
            if (bypass || ColonyStatsSettings.connectOnGameStartup)
            {
                InitializeClient(credentials);
            }
        }

        private static void ResetClient()
        {
            if (Client != null)
            {
                Client.Disconnect();
            }

            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            WebSocketClient customClient = new WebSocketClient(clientOptions);

            Client = new TwitchClient(customClient);
        }

        private static void InitializeClient(ConnectionCredentials credentials)
        {
            // Initialize the client with the credentials instance, and setting a default channel to connect to.
            Client.Initialize(credentials, ColonyStatsSettings.channel_username);

            // Bind callbacks to events
            Client.OnConnected += OnConnected;
            Client.OnJoinedChannel += OnJoinedChannel;
            Client.OnMessageReceived += OnMessageReceived;
            Client.OnWhisperReceived += OnWhisperReceived;
            //Client.OnChatCommandReceived += OnChatCommandReceived;
            Client.OnError += OnError;
            Client.Connect();
        }

        private static void OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
        }

        private static void OnConnected(object sender, OnConnectedArgs e)
        {
        }


        private static void OnError(object sender, object e)
        {
            Logger.Log(Logger.UNKNOWN, LogType.ERROR);
        }

        private static void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            if (ColonyStatsSettings.showWelcomeMessage)
                Client.SendMessage(e.Channel, "Twitch RimWorld pluggin Connected to Chat");
        }

        private static void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            bool canRunCommand = false;
            if (ColonyStatsSettings.subsOnlyCommands)
            {
                if (e.ChatMessage.IsSubscriber || e.ChatMessage.IsMe || e.ChatMessage.IsBroadcaster || e.ChatMessage.IsVip)
                {
                    canRunCommand = true;
                }
            }
            else
            {
                canRunCommand = true;
            }
            if (canRunCommand)
            {
                try
                {
                    Logger.Log("Message: " + $"{e.ChatMessage.DisplayName}: {e.ChatMessage.Message}", LogType.INFO);
                    if (Current.Game != null && TRANSLATORS != null && TRANSLATORS.Count > 0)
                    {
                        foreach (ITwitchTranslator receiver in TRANSLATORS)
                        {
                            Logger.Log("Evaluating " + receiver + " for message " + e.ChatMessage, LogType.INFO);
                            if (receiver.CanExecute(e.ChatMessage))
                            {
                                string message = receiver.ParseCommand(e.ChatMessage);
                                // If the name is incorrect or nothing is found the message will be null
                                Logger.Log("Message for Twitch: " + message, LogType.INFO);
                                if (message != null)
                                {
                                    if (message.Length > 500)
                                    {
                                        SendLargeChatMessage(message);
                                    }
                                    else
                                    {
                                        SendChatMessage(message);
                                    }
                                }
                                break;
                            }
                        }
                    }
                    Logger.Log("Finished processing", LogType.INFO);
                }
                catch (Exception ex)
                {
                    Logger.Log("Exception: " + ex.Message + "\n" + ex.StackTrace, Utilities.LogType.ERROR);
                }
            }
        }

        private static void SendLargeChatMessage(string message)
        {
            List<string> splitMessages = new List<string>();
            int maxLength = 490;
            Logger.Log("Message length " + message.Length, LogType.INFO);

            int i = 0;
            int tentativeCutLength = 0;
            int prevCut = 0;
            foreach (char character in message)
            {
                if (character == ' ')
                {
                    tentativeCutLength = i;
                }
                if (i == maxLength)
                {
                    splitMessages.Add(message.Substring(prevCut, tentativeCutLength));
                    prevCut = prevCut + tentativeCutLength;
                    i = 0;
                    tentativeCutLength = 0;

                }
                else
                {
                    i++;
                }
            }
            splitMessages.Add(message.Substring(prevCut, message.Length - prevCut));
            foreach (string splitMessage in splitMessages)
            {
                SendChatMessage(splitMessage);
                Thread.Sleep(100);
            }
        }

        // Method not being used currently
        /*private static void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            Logger.Log("Command: " + $"{e.Command.ChatMessage.DisplayName}: {e.Command.ChatMessage.Message}", LogType.INFO);

            Models.ChatCommand chatCommand = ChatCommandController.GetChatCommand(e.Command.CommandText);
            if (chatCommand != null)
            {
                chatCommand.TryExecute(e.Command as ITwitchCommand);
            }
        }*/

        public static void SendChatMessage(string message)
        {
            Client.SendMessage(Client.GetJoinedChannel(ColonyStatsSettings.channel_username), message);
        }
    }
}