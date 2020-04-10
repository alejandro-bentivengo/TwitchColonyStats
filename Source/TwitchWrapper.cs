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
        public static readonly Dictionary<string, DateTime> USER_TIMEOUT = new Dictionary<string, DateTime>();

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
            Client.Initialize(credentials, ColonyStatsSettings.channel_username);

            Client.OnConnected += OnConnected;
            Client.OnJoinedChannel += OnJoinedChannel;
            Client.OnMessageReceived += OnMessageReceived;
            Client.OnWhisperReceived += OnWhisperReceived;
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
            if (CanUserRunCommand(e))
            {
                try
                {
                    if (Current.Game != null && TRANSLATORS != null && TRANSLATORS.Count > 0)
                    {
                        foreach (ITwitchTranslator receiver in TRANSLATORS)
                        {
                            if (receiver.CanExecute(e.ChatMessage))
                            {
                                string message = receiver.ParseCommand(e.ChatMessage);
                                Logger.Log("Message for Twitch: " + message, LogType.INFO);
                                if (message != null)
                                {
                                    if (message.Length > 500 && (ColonyStatsSettings.enableSpammyMessages || ColonyStatsSettings.useWhispers))
                                    {
                                        SendLargeChatMessage(e, message);
                                    }
                                    else
                                    {
                                        SendChatMessage(e, message);
                                    }
                                    USER_TIMEOUT.SetOrAdd(e.ChatMessage.Username, DateTime.Now);
                                }
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log("Exception: " + ex.Message + "\n" + ex.StackTrace, LogType.ERROR);
                }
            }
        }

        private static bool CanUserRunCommand(OnMessageReceivedArgs e)
        {
            bool canRunCommand = false;
            DateTime userLastMessage = USER_TIMEOUT.TryGetValue(e.ChatMessage.Username);
            if ((userLastMessage != null && DateTime.Now.Subtract(userLastMessage).TotalSeconds > ColonyStatsSettings.userCommandTimeout) || ColonyStatsSettings.useWhispers)
            {
                if (ColonyStatsSettings.subsOnlyCommands)
                {
                    if (e.ChatMessage.IsSubscriber || e.ChatMessage.IsMe || e.ChatMessage.IsBroadcaster || e.ChatMessage.IsVip || e.ChatMessage.IsModerator)
                    {
                        canRunCommand = true;
                    }
                }
                else
                {
                    canRunCommand = true;
                }
            }
            return canRunCommand;
        }

        private static void SendLargeChatMessage(OnMessageReceivedArgs e, string message)
        {
            List<string> splitMessages = new List<string>();
            int maxLength = 500;
            int tentativeCutLength = 0;
            bool finishedProcessing = false;
            while (!finishedProcessing)
            {
                for (int i = 0; i < maxLength; i++)
                {
                    if (message[i].Equals(' '))
                    {
                        tentativeCutLength = i;
                    }
                }
                splitMessages.Add(message.Substring(0, tentativeCutLength));
                message = message.Substring(tentativeCutLength);
                if (message.Length <= maxLength)
                {
                    splitMessages.Add(message);
                    finishedProcessing = true;
                }
            }
            foreach (string splitMessage in splitMessages)
            {
                SendChatMessage(e, splitMessage);
                Thread.Sleep(100);
            }
        }

        public static void SendChatMessage(OnMessageReceivedArgs e, string message)
        {
            if (ColonyStatsSettings.useWhispers)
            {
                Client.SendWhisper(e.ChatMessage.Username, message);
            }
            else
            {
                Client.SendMessage(Client.GetJoinedChannel(ColonyStatsSettings.channel_username), message);
            }
        }
    }
}