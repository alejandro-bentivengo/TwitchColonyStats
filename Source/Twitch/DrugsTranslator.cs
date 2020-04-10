using Colonystats.Utilities;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    class DrugsTranslator : ITwitchTranslator
    {
        private static readonly string COMMAND = "drugs";

        public DrugsTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Use !drugs to return all non-medical drugs stored.";
        }

        public override string ParseCommand(ChatMessage msg)
        {
            List<Thing> drugs = ThingSelection.GetAllStoredNonMedicalDrugs();
            if (drugs != null && drugs.Count > 0)
            {
                return GetPrettyDrugs(drugs);
            }
            else
            {
                return "No drugs are currently in storage.";
            }
        }

        private string GetPrettyDrugs(List<Thing> drugs)
        {
            Dictionary<string, long> items = GetTotalCounts(drugs);
            string response = "Found " + items.Count + " non-medical drug types: ";
            foreach (KeyValuePair<string, long> item in items)
            {
                response += item.Value + " " + item.Key + " | ";
            }
            return response.Substring(0, response.Length - 3);
        }

        private Dictionary<string, long> GetTotalCounts(List<Thing> drugs)
        {
            Dictionary<string, long> items = new Dictionary<string, long>();
            foreach (Thing item in drugs)
            {
                long count = 0;
                if (items.ContainsKey(item.LabelNoCount))
                {
                    count = items.TryGetValue(item.LabelNoCount);
                }
                count += item.stackCount;
                items.SetOrAdd(item.LabelNoCount, count);
            }
            return items;
        }
    }
}
