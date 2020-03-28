﻿using System;
using TwitchLib.Client.Interfaces;
using Verse;

namespace Colonystats.Models
{
    public class ChatCommand : Def, IExposable
    {
        public string commandText;

        public bool enabled;

        public Type commandClass;

        public bool requiresMod;

        public bool requiresBroadcaster;

        public bool TryExecute(ITwitchCommand twitchCommand)
        {
            try
            {
                CommandMethod method = (CommandMethod)Activator.CreateInstance(commandClass, this);

                if (!method.CanExecute(twitchCommand)) return false;

                method.Execute(twitchCommand);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return true;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref commandText, "commandText", "helloworld");
            Scribe_Values.Look(ref enabled, "enabled", true);
            Scribe_Values.Look(ref commandClass, "commandClass", typeof(CommandMethod));
            Scribe_Values.Look(ref requiresMod, "requiresMod", false);
            Scribe_Values.Look(ref requiresBroadcaster, "requiresBroadcaster", false);
        }
    }
}