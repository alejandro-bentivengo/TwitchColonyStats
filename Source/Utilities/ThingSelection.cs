using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Colonystats.Utilities
{
    class ThingSelection
    {
        public static List<Thing> GetAllStoredThings()
        {            
            return Find.Maps.SelectMany(map => map.spawnedThings)
                .Where(thing => thing.Spawned &&
                                thing.IsInValidStorage())
                .OrderBy(thing => thing.thingIDNumber)
                .ToList();
        }

        public static List<Thing> GetAllStoredThingsThatContain(string name)
        {
            return Find.Maps.SelectMany(map => map.spawnedThings)
                .Where(thing => thing.Spawned &&
                                thing.IsInValidStorage() &&
                                thing.Label.ToLower().Contains(name))
                .OrderBy(thing => thing.thingIDNumber)
                .ToList();
        }
    }
}
