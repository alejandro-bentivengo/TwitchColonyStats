using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;

namespace Colonystats.Twitch
{
    sealed class HelpTranslator : ITwitchTranslator
    {

        internal static readonly List<string> REGISTERED_COMMANDS = new List<string>();

        private static readonly string COMMAND = "help";

        public HelpTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string ParseCommand(ChatMessage msg)
        {
            return GetAllCommands();
            throw new NotImplementedException();
        }

        private string GetAllCommands()
        {
            string response = "All commands must be executed with '!' in front: ";
            foreach (string command in REGISTERED_COMMANDS)
            {
                response += command + " | ";
            }
            return response.Substring(0, response.Length - 3);
        }
    }
}
