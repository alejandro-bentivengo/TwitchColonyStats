using TwitchLib.Client.Models;

namespace Colonystats.Twitch
{
    public abstract class ITwitchTranslator
    {

        public ITwitchTranslator(string command)
        {
            HelpTranslator.REGISTERED_COMMANDS.Add(command);
        }
        public abstract string ParseCommand(ChatMessage msg);

        public abstract bool CanExecute(ChatMessage msg);
    }

}
