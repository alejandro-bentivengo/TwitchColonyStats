using Colonystats.Utilities;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    sealed class ColonistTranslator : ITwitchTranslator
    {

        private static readonly string COMMAND = "colonists";

        public ColonistTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Use !colonists to get a list of the current colonists in the colony";
        }

        public override string ParseCommand(ChatMessage msg)
        {
            List<Pawn> colonists = ColonistSelection.AllColonistsInOrder;
            if (colonists.Count > 0)
            {
                return GetAllColonists(colonists);
            }
            return null;
        }

        private string GetAllColonists(List<Pawn> colonists)
        {
            string response = colonists.Count + " found: ";
            foreach (Pawn colonist in colonists)
            {
                response += colonist.Name.ToStringFull + " | ";
            }
            return response.Substring(0, response.Length - 3);
        }
    }
}
