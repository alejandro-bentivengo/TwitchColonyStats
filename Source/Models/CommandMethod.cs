using TwitchLib.Client.Interfaces;

namespace Colonystats.Models
{
    public class CommandMethod
    {
        public ChatCommand command;

        public CommandMethod(ChatCommand command)
        {
            this.command = command;
        }

        public virtual bool CanExecute(ITwitchCommand twitchCommand)
        {
            // If command not enabled
            if (!command.enabled) return false;

            // If command requires broadcaster status and message not from broadcaster
            if (command.requiresBroadcaster && twitchCommand.ChatMessage != null && !twitchCommand.ChatMessage.IsBroadcaster) return false;

            // If command requires moderator status and message not from broadcaster or moderator
            if (command.requiresMod && twitchCommand.ChatMessage != null && (!twitchCommand.ChatMessage.IsBroadcaster || !twitchCommand.ChatMessage.IsModerator)) return false;

            return true;
        }

        public virtual void Execute(ITwitchCommand twitchCommand)
        {

        }
    }
}