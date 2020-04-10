using TwitchLib.Client.Models;

namespace Colonystats.Twitch
{
    public abstract class ITwitchTranslator
    {

        private string command;

        public ITwitchTranslator(string command)
        {
            this.command = command;
        }

        public abstract string ParseCommand(ChatMessage msg);

        public abstract bool CanExecute(ChatMessage msg);

        public abstract string GetHelp();

        public string GetCommand()
        {
            return command;
        }

    }

}
