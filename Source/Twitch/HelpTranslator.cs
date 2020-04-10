using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;

namespace Colonystats.Twitch
{
    sealed class HelpTranslator : ITwitchTranslator
    {

        private static readonly string COMMAND = "tcshelp";

        public HelpTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Just execute !tcshelp to see the different registered commands, use !tcshelp {command} to see the command help";
        }

        public override string ParseCommand(ChatMessage msg)
        {
            string[] message = msg.Message.Split(' ');
            if (message.Length == 1)
            {
                return GetAllCommands();
            }
            else if (message.Length > 1)
            {
                return GetCommandDoc(message[1]);
            }
            return null;
        }

        private string GetCommandDoc(string v)
        {
            foreach (ITwitchTranslator command in TwitchWrapper.TRANSLATORS)
            {
                if (command.GetCommand().Equals(v))
                {
                    return command.GetHelp();
                }
            }
            return null;
        }

        private string GetAllCommands()
        {
            string response = "All commands must be executed with '!' in front: ";
            foreach (ITwitchTranslator command in TwitchWrapper.TRANSLATORS)
            {
                response += command.GetCommand() + " | ";
            }
            return response + "Use !tcshelp {command} to see the command help";
        }
    }
}
