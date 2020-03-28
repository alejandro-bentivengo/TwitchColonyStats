using Colonystats.Twitch;

namespace Colonystats.Utilities
{
    static class AnimalRegistrator
    {
        // All possible animals need to be added to the possible selectors under the animal translator
        // This will help to determine which ThingDef needs to be selected at the end
        public static void RegisterNewDef(string commandName, string defName)
        {
            AnimalTranslator.ANIMAL_DEFS.Add(commandName, defName);
        }

        public static void UnregisterDef(string commandName)
        {
            AnimalTranslator.ANIMAL_DEFS.Remove(commandName);
        }

        public static bool IsRegistered(string commandName)
        {
            return AnimalTranslator.ANIMAL_DEFS.ContainsKey(commandName);
        }

        internal static void RegisterDefaultDefs()
        {
            // Farm animals
            RegisterNewDef("donkey", "Donkey");
            RegisterNewDef("pig", "Pig");
            RegisterNewDef("cow", "Cow");
            RegisterNewDef("alpaca", "Alpaca");
            RegisterNewDef("duck", "Duck");
            RegisterNewDef("bison", "Bison");
            RegisterNewDef("goat", "Goat");
            RegisterNewDef("goose", "Goose");
            RegisterNewDef("sheep", "Sheep");
            RegisterNewDef("horse", "Horse");
            RegisterNewDef("yak", "Yak");
            RegisterNewDef("guineapig", "GuineaPig");

            // Pet Animals
            RegisterNewDef("yorkshire", "YorkshireTerrier");
            RegisterNewDef("husky", "Husky");
            RegisterNewDef("labrador", "LabradorRetriever");
            RegisterNewDef("cat", "Cat");

            // Arid Animals
            RegisterNewDef("muffalo", "Muffalo");
            RegisterNewDef("gazelle", "Gazelle");
            RegisterNewDef("iguana", "Iguana");
            RegisterNewDef("dromedary", "Dromedary");

            // Bear Animals
            RegisterNewDef("grizzlybear", "Bear_Grizzly");
            RegisterNewDef("polarbear", "Bear_Polar");

            // BigCat Animals
            RegisterNewDef("cougar", "Cougar");
            RegisterNewDef("panther", "Panther");
            RegisterNewDef("lynx", "Lynx");

            // BigBird Animals
            RegisterNewDef("cassowary", "Cassowary");
            RegisterNewDef("emu", "Emu");
            RegisterNewDef("ostrich", "Ostrich");
            RegisterNewDef("turkey", "Turkey");

            // Giant Animals
            RegisterNewDef("rhinoceros", "Rhinoceros");
            RegisterNewDef("elephant", "Elephant");
            RegisterNewDef("megasloth", "Megasloth");
            RegisterNewDef("thrumbo", "Thrumbo");

            // Hare Animals
            RegisterNewDef("hare", "Hare");
            RegisterNewDef("snowhare", "Snowhare");

            // Insect Animals
            RegisterNewDef("megascarab", "Megascarab");
            RegisterNewDef("spelopede", "Spelopede");
            RegisterNewDef("megaspider", "Megaspider");

            // Rodent Animals
            RegisterNewDef("squirrel", "Squirrel");
            RegisterNewDef("alphabeaver", "Alphabeaver");
            RegisterNewDef("capybara", "Capybara");
            RegisterNewDef("chinchilla", "Chinchilla");
            RegisterNewDef("boomrat", "Boomrat");
            RegisterNewDef("raccoon", "Raccoon");
            RegisterNewDef("rat", "Rat");

            // Temperate Animals
            RegisterNewDef("deer", "Deer");
            RegisterNewDef("ibex", "Ibex");
            RegisterNewDef("elk", "Elk");
            RegisterNewDef("caribou", "Caribou");
            RegisterNewDef("wildboar", "WildBoar");
            RegisterNewDef("tortoise", "Tortoise");

            // Tropical Animals
            RegisterNewDef("cobra", "Cobra");
            RegisterNewDef("monkey", "Monkey");
            RegisterNewDef("boomalope", "Boomalope");

            // Wild Canine Animals
            RegisterNewDef("warg", "Warg");
            RegisterNewDef("timberwolf", "Wolf_Timber");
            RegisterNewDef("arcticwolf", "Wolf_Arctic");
            RegisterNewDef("fennecfox", "Fox_Fennec");
            RegisterNewDef("redfox", "Fox_Red");
            RegisterNewDef("arcticfox", "Fox_Arctic");
        }

    }
}
