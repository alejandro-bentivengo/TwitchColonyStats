using Colonystats.Utilities;
using RimWorld;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    sealed class ColonistDetailsTranslator : ITwitchTranslator
    {
        private static readonly string COMMAND = "colonist";

        public ColonistDetailsTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Use !colonist {colonist nickname} to return the colonist details";
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
                    return GetPrettyColonist(matching[0]);
                }
            }
            return null;
        }

        private string GetPrettyColonist(Pawn pawn)
        {
            return "Colonist Stats for " + pawn.Name + "> | " +
            "Age: " + pawn.ageTracker.AgeBiologicalYears + " | " +
            "Gender: " + pawn.GetGenderLabel() + " | " +
            "Needs> " +
            "Food: " + Math.Round(pawn.needs.TryGetNeed<Need_Food>().CurLevelPercentage * 100) + "% | " +
            "Rest: " + Math.Round(pawn.needs.TryGetNeed<Need_Rest>().CurLevelPercentage * 100) + "% | " +
            "Recreation: " + Math.Round(pawn.needs.TryGetNeed<Need_Joy>().CurLevelPercentage * 100) + "% | " +
            "Beauty: " + Math.Round(pawn.needs.TryGetNeed<Need_Beauty>().CurLevelPercentage * 100) + "% | " +
            "Comfort: " + Math.Round(pawn.needs.TryGetNeed<Need_Comfort>().CurLevelPercentage * 100) + "% | " +
            "Recreation: " + Math.Round(pawn.needs.TryGetNeed<Need_Outdoors>().CurLevelPercentage * 100) + "%";
        }
    }
}
