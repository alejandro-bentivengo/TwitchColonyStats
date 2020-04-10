
using Colonystats.Utilities;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    class HealthTranslator : ITwitchTranslator
    {
        private static readonly string COMMAND = "health";

        public HealthTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Use !health {colonist name} to return the colonist health status.";
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
                    return GetPrettyHealth(matching[0]);
                }
                else
                {
                    return "Colonist " + name + " not found.";
                }
            }
            return null;
        }

        private string GetPrettyHealth(Pawn pawn)
        {
            string response = "Health for " + pawn.Name.ToStringShort + ": ";
            Dictionary<string, Dictionary<string, long>> conditions = new Dictionary<string, Dictionary<string, long>>();
            foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
            {
                if (hediff.Visible)
                {
                    string bodypart = hediff.Part == null ? "Wholebody" : hediff.Part.def.defName;
                    Dictionary<string, long> afflictions = new Dictionary<string, long>();
                    if (conditions.ContainsKey(bodypart))
                    {
                        conditions.TryGetValue(bodypart, out afflictions);
                    }
                    long afflictionCount = 0;
                    if (afflictions.ContainsKey(hediff.Label))
                    {
                        afflictions.TryGetValue(hediff.Label, out afflictionCount);
                    }
                    afflictions.SetOrAdd(hediff.Label, afflictionCount + 1);
                    conditions.SetOrAdd(bodypart, afflictions);
                }
            }
            foreach (string bodypart in conditions.Keys)
            {
                conditions.TryGetValue(bodypart, out Dictionary<string, long> afflictions);
                response += bodypart + " - ";
                foreach (string affliction in afflictions.Keys)
                {
                    afflictions.TryGetValue(affliction, out long count);
                    response += affliction + (count == 1 ? "" : " (x" + count + ")") + ", ";
                }
            }
            return response.Substring(0, response.Length - 2);
        }
    }
}
