using Colonystats.Utilities;
using System;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    class CountTranslator : ITwitchTranslator
    {

        private static readonly string COMMAND = "count";

        public CountTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Use !count {pawn type} to get the total count of said type in the colony. Accepts all animal types and colonist as parameter.";
        }

        public override string ParseCommand(ChatMessage msg)
        {
            string[] message = msg.Message.Split(' ');
            if (message.Length > 1)
            {
                string type = msg.Message.Replace("!" + COMMAND + " ", "");
                if (type.ToLower().Equals("colonist"))
                {
                    return GetPrettyColonistCount();
                }
                else if (AnimalTranslator.ANIMAL_DEFS.ContainsKey(type))
                {
                    return GetPrettyAnimalCount(AnimalTranslator.ANIMAL_DEFS.TryGetValue(type));
                }
                else
                {
                    return "Def " + type + " not found.";
                }
            }
            return null;
        }

        private string GetPrettyAnimalCount(string animalType)
        {
            return "Total count of " + animalType + " in the colony: " + AnimalSelection.GetAllTameAnimalsInOrderWithDef(animalType).Count;
        }

        private string GetPrettyColonistCount()
        {
            return "Total colonists: " + ColonistSelection.AllColonistsInOrder.Count;
        }
    }
}
