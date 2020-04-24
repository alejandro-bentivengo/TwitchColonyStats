using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Colonystats.Utilities
{
    static class AnimalSelection
    {
        public static List<Pawn> AllTameAnimalsInOrder
        {
            get
            {
                return Find.Maps.SelectMany(map => map.mapPawns.AllPawnsSpawned)
                    .Where(pawn => pawn.Faction == Faction.OfPlayer &&
                                   pawn.RaceProps.Animal &&
                                   ThingSelectionUtility.SelectableByHotkey(pawn))
                    .OrderBy(pawn => pawn.Map?.uniqueID ?? pawn.GetCaravan()?.ID + 500 ?? -1)
                    .ThenBy(pawn => pawn.kindDef.label)
                    .ThenBy(pawn => pawn.Label)
                    .ToList();
            }
        }

        public static List<Pawn> GetAllTameAnimalsInOrderWithDef(string def)
        {
            return Find.Maps.SelectMany(map => map.mapPawns.AllPawnsSpawned)
                .Where(pawn => pawn.Faction == Faction.OfPlayer &&
                               pawn.RaceProps.Animal &&
                               ThingSelectionUtility.SelectableByHotkey(pawn) &&
                               pawn.def.defName.Equals(def))
                .OrderBy(pawn => pawn.Map?.uniqueID ?? pawn.GetCaravan()?.ID + 500 ?? -1)
                .ThenBy(pawn => pawn.kindDef.label)
                .ThenBy(pawn => pawn.Label)
                .ToList();
        }

        public static List<Pawn> GetAllWildAnimalsInOrderWithDef(string def)
        {
            return Find.Maps.SelectMany(map => map.mapPawns.AllPawnsSpawned)
                .Where(pawn => pawn.AnimalOrWildMan() &&
                               pawn.Faction == null &&
                               pawn.def.defName.Equals(def))
                .OrderBy(pawn => pawn.Map?.uniqueID ?? pawn.GetCaravan()?.ID + 500 ?? -1)
                .ThenBy(pawn => pawn.kindDef.label)
                .ThenBy(pawn => pawn.Label)
                .ToList();
        }

        public static List<Pawn> GetAllTameAnimalsInOrderWithName(string name)
        {
            return Find.Maps.SelectMany(map => map.mapPawns.AllPawnsSpawned)
                .Where(pawn => pawn.Faction == Faction.OfPlayer &&
                               pawn.RaceProps.Animal &&
                               ThingSelectionUtility.SelectableByHotkey(pawn) &&
                               pawn.Name.ToStringFull.ToLower().Equals(name.ToLower()))
                .OrderBy(pawn => pawn.Map?.uniqueID ?? pawn.GetCaravan()?.ID + 500 ?? -1)
                .ThenBy(pawn => pawn.kindDef.label)
                .ThenBy(pawn => pawn.Label)
                .ToList();
        }
    }
}
