using Colonystats.Utilities;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    sealed class AnimalTranslator : ITwitchTranslator
    {

        // all animal defs will be stored here
        // Unfortunately I didnt find a way to get all animal definitions off the bat
        // Im certain there has to be a way, I just didnt find one
        internal static readonly Dictionary<string, string> ANIMAL_DEFS = new Dictionary<string, string>();

        private static readonly string COMMAND = "animals";

        public AnimalTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string ParseCommand(ChatMessage msg)
        {
            string[] message = msg.Message.Split(' ');
            if (message.Length > 1)
            {
                string def = ANIMAL_DEFS.TryGetValue(message[1]);
                if (def != null)
                {
                    List<Pawn> matching = AnimalSelection.GetAllTameAnimalsInOrderWithDef(def);
                    if (matching != null && matching.Count > 0)
                    {
                        return GetPrettyAnimalList(matching);
                    }
                    else
                    {
                        return "No tamed animals " + def + " found";
                    }
                }
                else
                {
                    return "Def " + def + " not found";
                }
            }
            return null;
        }

        private string GetPrettyAnimalList(List<Pawn> matching)
        {
            string response = "Found " + matching.Count + " animals: ";
            foreach (Pawn pawn in matching)
            {
                response += pawn.Name.ToStringFull + " | ";
            }
            return response.Substring(0, response.Length - 3);
        }

        private string GetAllDefs()
        {
            string response = "Possible Animal Types: ";
            foreach (string def in ANIMAL_DEFS.Keys)
            {
                response += def + " | ";
            }
            return response.Substring(0, response.Length - 3);
        }

        public override string GetHelp()
        {
            return "Use !animals {animal type} to return all tamed animals of that type in the colony. " + GetAllDefs();
        }
    }
}
