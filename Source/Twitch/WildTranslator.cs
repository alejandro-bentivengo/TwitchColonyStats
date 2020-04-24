using Colonystats.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    class WildTranslator : ITwitchTranslator
    {
        private static readonly string COMMAND = "wild";

        public WildTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Use !wild {animal type} to return a count of the wild animals of that type.";
        }

        public override string ParseCommand(ChatMessage msg)
        {
            string[] message = msg.Message.Split(' ');
            if (message.Length > 1)
            {
                string def = AnimalTranslator.ANIMAL_DEFS.TryGetValue(message[1]);
                if (def != null)
                {
                    List<Pawn> matching = AnimalSelection.GetAllWildAnimalsInOrderWithDef(def);
                    if (matching != null && matching.Count > 0)
                    {
                        return GetPrettyAnimalList(matching, def);
                    }
                    else
                    {
                        return "No wild animals " + def + " found";
                    }
                }
                else
                {
                    return "The animal " + def + " was not found. The animal type is probably misspelled.";
                }
            }
            return null;
        }

        private string GetPrettyAnimalList(List<Pawn> matching, string def)
        {
            return "Found " + matching.Count + " wild animals of type " + def;
        }
    }
}
