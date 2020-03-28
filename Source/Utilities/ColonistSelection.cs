using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Colonystats.Utilities
{
    static class ColonistSelection
    {

        public static List<Pawn> AllColonistsInOrder
        {
            get
            {

                return Find.World.worldPawns.AllPawnsAlive
                    .Where(pawn => pawn.Faction == Faction.OfPlayer &&
                                   pawn.IsColonist)
                    .OrderBy(pawn => pawn.Map?.uniqueID ?? pawn.GetCaravan()?.ID + 500 ?? -1)
                    .ThenBy(pawn => pawn.kindDef.label)
                    .ThenBy(pawn => pawn.Label)
                    .Concat(
                        Find.Maps.SelectMany(map => map.mapPawns.AllPawnsSpawned)
                            .Where(pawn => pawn.Faction == Faction.OfPlayer &&
                                           pawn.IsColonist)
                            .OrderBy(pawn => pawn.Map?.uniqueID ?? pawn.GetCaravan()?.ID + 500 ?? -1)
                            .ThenBy(pawn => pawn.kindDef.label)
                            .ThenBy(pawn => pawn.Label)
                    )
                    .ToList();
            }
        }

        public static List<Pawn> GetAllColonistsInOrderWithName(string name)
        {
            return Find.World.worldPawns.AllPawnsAlive
                .Where(pawn => pawn.Faction == Faction.OfPlayer &&
                                pawn.IsColonist &&
                                pawn.Name.ToStringFull.Equals(name))
                .OrderBy(pawn => pawn.Map?.uniqueID ?? pawn.GetCaravan()?.ID + 500 ?? -1)
                .ThenBy(pawn => pawn.kindDef.label)
                .ThenBy(pawn => pawn.Label)
                .Concat(
                    Find.Maps.SelectMany(map => map.mapPawns.AllPawnsSpawned)
                        .Where(pawn => pawn.Faction == Faction.OfPlayer &&
                                        pawn.IsColonist &&
                                        pawn.Name.ToStringFull.Equals(name))
                        .OrderBy(pawn => pawn.Map?.uniqueID ?? pawn.GetCaravan()?.ID + 500 ?? -1)
                        .ThenBy(pawn => pawn.kindDef.label)
                        .ThenBy(pawn => pawn.Label)
                )
                .ToList();
        }

    }
}
