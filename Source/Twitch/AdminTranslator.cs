using System;
using TwitchLib.Client.Models;

namespace Colonystats.Twitch
{
    class AdminTranslator : ITwitchTranslator
    {

        private static readonly string COMMAND = "admin";

        public AdminTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return (msg.IsBroadcaster || msg.IsModerator) && msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "See workshop page for details";
        }

        public override string ParseCommand(ChatMessage msg)
        {
            string[] message = msg.Message.Split(' ');
            if (message.Length > 1)
            {
                return DoAdminStuff(message);
            }
            return null;
        }

        private string DoAdminStuff(string[] adminCommand)
        {
            string reply = null;
            switch (adminCommand[1])
            {
                case "timeout":
                    long time;
                    if (adminCommand.Length > 2 && long.TryParse(adminCommand[2], out time))
                    {
                        if (time >= 0 && time <= 20)
                        {
                            ColonyStatsSettings.userCommandTimeout = time;
                            reply = "User message timeout changed to " + time + " seconds";
                        }
                    }
                    break;
                case "spam":
                    ColonyStatsSettings.enableSpammyMessages = !ColonyStatsSettings.enableSpammyMessages;
                    reply = "Allow Spam messages changed to " + ColonyStatsSettings.enableSpammyMessages;
                    break;
                case "subs":
                    ColonyStatsSettings.subsOnlyCommands = !ColonyStatsSettings.subsOnlyCommands;
                    reply = "Subscriber only mode change to " + ColonyStatsSettings.subsOnlyCommands;
                    break;
                case "whispers":
                    ColonyStatsSettings.useWhispers = !ColonyStatsSettings.useWhispers;
                    reply = "Send replies as whispers changed to " + ColonyStatsSettings.useWhispers;
                    break;
            }
            return reply;
        }
    }
}
