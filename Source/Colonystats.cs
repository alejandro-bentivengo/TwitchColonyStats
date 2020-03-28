using Colonystats.Utilities;
using UnityEngine;
using Verse;

namespace Colonystats
{
    public class ColonyStats : Mod
    {

        private static ColonyStatsSettings settings;

        public ColonyStats(ModContentPack content) : base(content)
        {
            TranslatorRegistrator.AddDefault();
            AnimalRegistrator.RegisterDefaultDefs();
            settings = GetSettings<ColonyStatsSettings>();
        }

        public override string SettingsCategory() => "Twitch Colony Stats";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }

    }


    [StaticConstructorOnStartup]
    public static class Startup
    {
        static Startup()
        {
            TwitchWrapper.StartAsync();
        }
    }

}