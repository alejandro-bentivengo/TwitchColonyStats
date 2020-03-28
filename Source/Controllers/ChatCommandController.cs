using Colonystats.Models;
using System;
using System.Linq;
using Verse;

namespace Colonystats.Controllers
{
    public static class ChatCommandController
    {
        public static ChatCommand GetChatCommand(string commandText)
        {
            return DefDatabase<ChatCommand>.AllDefs.ToList().Find(cc => string.Equals(cc.commandText, commandText, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}