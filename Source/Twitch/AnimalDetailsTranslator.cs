using Colonystats.Utilities;
using RimWorld;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    /**
     * This Translator will lookup a specific animal based on his name
     * !animal <animalName>
     * 
     * Ie: !animal Bethany
     * 
     */
    sealed class AnimalDetailsTranslator : ITwitchTranslator
    {

        private static readonly string COMMAND = "animal";

        public AnimalDetailsTranslator() : base(COMMAND)
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
                string name = msg.Message.Replace("!" + COMMAND + " ", "");
                List<Pawn> matching = AnimalSelection.GetAllTameAnimalsInOrderWithName(name);
                if (matching != null && matching.Count > 0)
                {
                    return GetPrettyAnimal(matching[0]);
                }
            }
            return null;
        }

        private string GetPrettyAnimal(Pawn pawn)
        {
            return "Animal Stats for " + pawn.Name + " | " +
            "Age: " + pawn.ageTracker.AgeBiologicalYears + " | " +
            "Gender: " + pawn.GetGenderLabel() + " | " +
            "Needs " +
            "Rest: " + Math.Round(pawn.needs.TryGetNeed<Need_Rest>().CurLevelPercentage * 100) + "% | " +
            "Food: " + Math.Round(pawn.needs.TryGetNeed<Need_Food>().CurLevelPercentage * 100) + "%";
        }

    }
}
