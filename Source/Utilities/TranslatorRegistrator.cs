using Colonystats.Twitch;

namespace Colonystats.Utilities
{
    static class TranslatorRegistrator
    {

        internal static void AddDefault()
        {
            Logger.Log("Registering default Twitch translators", LogType.INFO);
            AddTranslator(new AnimalDetailsTranslator());
            AddTranslator(new AnimalTranslator());
            AddTranslator(new ColonistTranslator());
            AddTranslator(new ColonistDetailsTranslator());
            AddTranslator(new HelpTranslator());
            AddTranslator(new SkillTranslator());
            AddTranslator(new AdminTranslator());
            AddTranslator(new CountTranslator());
            AddTranslator(new ItemTranslator());
            AddTranslator(new TraitsTranslator());
            AddTranslator(new HealthTranslator());
            AddTranslator(new DrugsTranslator());
            Logger.Log("Finishing Registering default Twitch translators", LogType.INFO);
        }

        public static void AddTranslator(ITwitchTranslator translator)
        {
            Logger.Log("Adding translator " + translator.GetType(), LogType.INFO);
            TwitchWrapper.TRANSLATORS.Add(translator);
        }

        public static void RemoveAllTypeTranslator(ITwitchTranslator translator)
        {
            Logger.Log("Removing translators " + translator.GetType(), LogType.INFO);
            TwitchWrapper.TRANSLATORS.RemoveAll((t1) => t1.GetType().Equals(translator.GetType()));
        }

        public static void RemoveTranslator(ITwitchTranslator translator)
        {
            Logger.Log("Removing translator " + translator, LogType.INFO);
            TwitchWrapper.TRANSLATORS.Remove(translator);
        }

    }
}
