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
    class ItemTranslator : ITwitchTranslator
    {
        private static readonly string COMMAND = "item";

        public ItemTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Use !item {item name} to find how many there are currently in storage.";
        }

        public override string ParseCommand(ChatMessage msg)
        {
            string[] message = msg.Message.Split(' ');
            if (message.Length > 1)
            {
                string itemName = msg.Message.Replace("!" + COMMAND + " ", "");
                if (itemName.Length > 4)
                {
                    List<Thing> matching = ThingSelection.GetAllStoredThingsThatContain(itemName);
                    if (matching != null && matching.Count > 0)
                    {
                        return GetPrettyItemCount(matching);
                    }
                }
            }
            return null;
        }

        private string GetPrettyItemCount(List<Thing> matching)
        {
            Dictionary<string, long> items = GetTotalCounts(matching);
            string response = "Found " + items.Count + " items matching: ";
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
                if (items.ContainsKey(item.Label))
                {
                    count = items.TryGetValue(item.Label);
                }
                count++;
                items.SetOrAdd(item.Label, count);
            }
            return items;
        }
    }
}
