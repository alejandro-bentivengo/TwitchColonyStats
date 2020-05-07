using Colonystats.Utilities;
using System.Collections.Generic;
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
            return "Use !item {item name} to find how many there are currently in storage. Put the name in \" to exact match the name. Case insensitive. Search needs to be at least 4 character long.";
        }

        public override string ParseCommand(ChatMessage msg)
        {
            string[] message = msg.Message.Split(' ');
            if (message.Length > 1)
            {
                string itemName = msg.Message.Replace("!" + COMMAND + " ", "");
                if (itemName.Length >= 4)
                {
                    List<Thing> matching = null;
                    if (itemName.StartsWith("\"") && itemName.EndsWith("\""))
                    {
                        matching = ThingSelection.GetAllStoredThingsThatExactMatch(itemName.Replace("\"", ""));
                    }
                    else
                    {
                        matching = ThingSelection.GetAllStoredThingsThatContain(itemName);
                    }
                    if (matching != null && matching.Count > 0)
                    {
                        return GetPrettyItemCount(matching);
                    }
                    else
                    {
                        return "The item " + itemName + " was not found.";
                    }
                }
                else
                {
                    return "The item name is too short, it must be at least 4 characters.";
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
