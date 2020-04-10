using Colonystats.Utilities;
using RimWorld;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    class TraitsTranslator : ITwitchTranslator
    {
        private static readonly string COMMAND = "traits";

        public TraitsTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Use !traits {colonist name} to return all colonist traits.";
        }

        public override string ParseCommand(ChatMessage msg)
        {
            string[] message = msg.Message.Split(' ');
            if (message.Length > 1)
            {
                string name = msg.Message.Replace("!" + COMMAND + " ", "");
                List<Pawn> matching = ColonistSelection.GetAllColonistsInOrderWithName(name);
                if (matching != null && matching.Count > 0)
                {
                    return GetPrettyTraits(matching[0]);
                }
                else
                {
                    return "Colonist " + name + " not found.";
                }
            }
            return null;
        }

        private string GetPrettyTraits(Pawn pawn)
        {
            string response = "Traits for " + pawn.Name.ToStringShort + ": ";
            foreach (Trait trait in pawn.story.traits.allTraits)
            {
                response += trait.Label + " | ";
            }
            response += "Childhood: " + pawn.story.childhood.title + " | ";
            response += "Adulthood: " + pawn.story.adulthood.title;
            return response;
        }
    }
}
