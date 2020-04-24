using Colonystats.Utilities;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    class MealTranslator : ITwitchTranslator
    {

        private static readonly string COMMAND = "meals";
        public MealTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Use !meals to find how many meals there are currently in storage. Finds only meals, not raw food.";
        }

        public override string ParseCommand(ChatMessage msg)
        {
            List<Thing> matching = ThingSelection.GetAllStoredFood();
            if (matching != null && matching.Count > 0)
            {
                return GetPrettyItemCount(matching);
            }
            return null;
        }

        private string GetPrettyItemCount(List<Thing> matching)
        {
            Dictionary<string, long> items = GetTotalCounts(matching);
            string response = "Found " + items.Count + " meals: ";
            foreach (KeyValuePair<string, long> item in items)
            {
                response += item.Value + " " + item.Key + " | ";
            }
            return response.Substring(0, response.Length - 3);
        }

        private Dictionary<string, long> GetTotalCounts(List<Thing> matching)
        {
            Dictionary<string, long> items = new Dictionary<string, long>();
            foreach (Thing item in matching)
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
