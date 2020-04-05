using Colonystats.Utilities;
using RimWorld;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using Verse;

namespace Colonystats.Twitch
{
    sealed class SkillTranslator : ITwitchTranslator
    {

        private static readonly string COMMAND = "skills";

        public SkillTranslator() : base(COMMAND)
        {
        }

        public override bool CanExecute(ChatMessage msg)
        {
            return msg.Message.Length > 0 && msg.Message.Split(' ')[0].Equals("!" + COMMAND);
        }

        public override string GetHelp()
        {
            return "Use !skills {colonist nickname} to get a detail of the colonist skills";
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
                    return GetPrettySkills(matching[0]);
                }
            }
            return null;
        }

        private string GetPrettySkills(Pawn pawn)
        {
            string response = "Skills for " + pawn.Name + ": ";
            foreach (SkillRecord skill in pawn.skills.skills)
            {
                response += skill.def.defName + ": " + skill.Level + " | ";
            }
            return response.Substring(0, response.Length - 3);
        }
    }
}
