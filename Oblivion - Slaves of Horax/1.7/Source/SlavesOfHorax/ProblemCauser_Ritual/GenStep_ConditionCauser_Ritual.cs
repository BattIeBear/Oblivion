using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Oblivion_SlavesOfHorax
{
    public class GenStep_ConditionCauser_Ritual : GenStep_Scatterer
    {
        Pawn ritualLeader;

        private const int RitualAreaSize = 11;

        private GenStepParams currentParams;

        public override int SeedPart => 1068345640;

        protected override bool CanScatterAt(IntVec3 c, Map map)
        {
            if (!base.CanScatterAt(c, map))
            {
                return false;
            }

            CellRect rect = CellRect.CenteredOn(c, RitualAreaSize, RitualAreaSize);

            if (!rect.InBounds(map))
            {
                return false;
            }

            if (MapGenerator.TryGetVar("UsedRects", out List<CellRect> usedRects) &&
                usedRects.Any(x => rect.Overlaps(x)))
            {
                return false;
            }

            foreach (IntVec3 cell in rect.Cells)
            {
                if (!GenConstruct.CanBuildOnTerrain(
                        currentParams.sitePart.conditionCauser.def,
                        cell,
                        map,
                        Rot4.North))
                {
                    return false;
                }

                foreach (Thing thing in cell.GetThingList(map))
                {
                    if (thing.def.building != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override void Generate(Map map, GenStepParams parms)
        {
            currentParams = parms;
            count = 1;
            base.Generate(map, parms);
        }

        protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
        {
            Faction faction = map.ParentFaction != null && map.ParentFaction != Faction.OfPlayer ? map.ParentFaction : Find.FactionManager.RandomEnemyFaction();

            List<CellRect> usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");

            CellRect ritualRect = CellRect.CenteredOn(loc, RitualAreaSize, RitualAreaSize).ClipInsideMap(map);

            SitePart sitePart = parms.sitePart;

            sitePart.conditionCauserWasSpawned = true;

            ResolveParams rp = new ResolveParams
            {
                rect = ritualRect,
                faction = faction,
                conditionCauser = sitePart.conditionCauser
            };

            BaseGen.globalSettings.map = map;
            BaseGen.symbolStack.Push("conditionCauserRoom", rp);
            BaseGen.Generate();

            DecorateRitualSite(map, ritualRect, faction);

            CompCauseGameCondition_Ritual compCauser = sitePart.conditionCauser.TryGetComp<CompCauseGameCondition_Ritual>();

            SpawnCultists(map, sitePart, ritualRect, faction, compCauser);

            MapGenerator.SetVar("RectOfInterest", ritualRect);
            usedRects.Add(ritualRect);
        }

        private void DecorateRitualSite(Map map, CellRect rect, Faction faction)
        {
            IntVec3 center = rect.CenterCell;

            SpawnRitualCircle(map, center);
            SpawnInteriorTorchLampsAndColumns(map, rect, faction);
            SpawnBeacons(map, rect, faction);
            SpawnCorpses(map, rect, faction);

            rect = rect.ExpandedBy(3);
            SpawnBlood(map, rect);
            SpawnExteriorDefenses(map, rect, faction);
            //SpawnStatues(map, rect, faction);
        }

        private Thing StyledThing(ThingDef def, Faction faction, ThingDef stuff = null)
        {
            Thing thing;
            if (stuff != null)
            {
                thing = ThingMaker.MakeThing(def, stuff);
            }
            else
            {
                thing = ThingMaker.MakeThing(def);
            }
            if (faction != null)
            {
                thing.SetFaction(faction);
                if (ModsConfig.IdeologyActive)
                {
                    if (faction.ideos?.PrimaryIdeo != null)
                    {
                        ThingStyleDef style = faction.ideos.PrimaryIdeo.GetStyleFor(def);
                        if (style != null)
                        {
                            thing.SetStyleDef(style);
                        }
                    }
                }
            }
            return thing;
        }

        private Pawn CreateVictim(Map map, Faction faction)
        {
            Faction victimFaction = (from f in Find.FactionManager.GetFactions(allowHidden: false, allowDefeated: false, allowNonHumanlike: false)
                                     where !f.IsPlayer && f.HostileTo(faction)
                                     select f).RandomElementWithFallback(faction);

            PawnKindDef slave = RimWorld.PawnKindDefOf.Slave;
            Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(slave, victimFaction, PawnGenerationContext.NonPlayer, map.Tile, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: false, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: false));

            return pawn;
        }

        private void KillVictim(Pawn pawn)
        {
            for (int i = 0; i < 2; i++)
            {
                if (Rand.Value % 2 == 0)
                {
                    pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, Rand.Range(10f, 25f)));
                }
                else
                {
                    pawn.TakeDamage(new DamageInfo(DamageDefOf.Stab, Rand.Range(10f, 25f)));
                }
            }

            if (pawn.Dead == false)
            {
                pawn.Kill(null, null);
            }
            pawn.Corpse.Age += Rand.Range(0, 300000);
        }

        private void SpawnRitualCircle(Map map, IntVec3 center)
        {
            Thing ritualCircle = ThingMaker.MakeThing(ThingDef.Named("BattIeBear_HoraxRitualSpot"));
            GenSpawn.Spawn(ritualCircle, center, map);
        }

        private void SpawnInteriorTorchLampsAndColumns(Map map, CellRect rect, Faction faction)
        {
            GenSpawn.Spawn(StyledThing(ThingDef.Named("TorchWallLamp"), faction), new IntVec3(rect.minX + 1, 0, rect.maxZ - 2), map, Rot4.West);
            GenSpawn.Spawn(StyledThing(ThingDef.Named("TorchWallLamp"), faction), new IntVec3(rect.minX + 1, 0, rect.minZ + 2), map, Rot4.West);
            GenSpawn.Spawn(StyledThing(ThingDef.Named("TorchWallLamp"), faction), new IntVec3(rect.maxX - 1, 0, rect.maxZ - 2), map, Rot4.East);
            GenSpawn.Spawn(StyledThing(ThingDef.Named("TorchWallLamp"), faction), new IntVec3(rect.maxX - 1, 0, rect.minZ + 2), map, Rot4.East);

            ThingDef columnMaterial = BaseGenUtility.CheapStuffFor(ThingDefOf.Column, faction);
            int skip = Rand.Range(0, 8);
            for (int i = 0; i < 4; i++)
            {
                if (i == skip)
                {
                    continue;
                }
                switch (i)
                {
                    case 0:
                        GenSpawn.Spawn(StyledThing(ThingDefOf.Column, faction, columnMaterial), new IntVec3(rect.minX + 2, 0, rect.maxZ - 2), map, Rot4.South);
                        break;
                    case 1:
                        GenSpawn.Spawn(StyledThing(ThingDefOf.Column, faction, columnMaterial), new IntVec3(rect.minX + 2, 0, rect.minZ + 2), map, Rot4.South);
                        break;
                    case 2:
                        GenSpawn.Spawn(StyledThing(ThingDefOf.Column, faction, columnMaterial), new IntVec3(rect.maxX - 2, 0, rect.maxZ - 2), map, Rot4.South);
                        break;
                    case 3:
                        GenSpawn.Spawn(StyledThing(ThingDefOf.Column, faction, columnMaterial), new IntVec3(rect.maxX - 2, 0, rect.minZ + 2), map, Rot4.South);
                        break;
                }
            }
        }

        private void SpawnBeacons(Map map, CellRect rect, Faction faction)
        {
            List<IntVec3> beaconCells = new List<IntVec3>();
            int beacons = Rand.RangeInclusive(0, 4);
            int iterations = 0;
            for (int i = 0; i < beacons && iterations < 20; i++)
            {
                IntVec3 cell = CellFinder.RandomClosewalkCellNear(rect.CenterCell, map, (rect.Width - 1) / 2);

                if (!cell.Standable(map))
                {
                    i--;
                    iterations++;
                    continue;
                }
                if (cell.AdjacentTo(rect.CenterCell) || cell == rect.CenterCell || cell == rect.CenterCell + (IntVec3.North * 2))
                {
                    i--;
                    iterations++;
                    continue;
                }

                bool tooClose = false;
                foreach (IntVec3 beaconCell in beaconCells)
                {
                    if (cell.AdjacentTo(beaconCell))
                    {
                        tooClose = true;
                    }
                }

                if (tooClose)
                {
                    i--;
                    iterations++;
                    continue;
                }

                beaconCells.Add(cell);
                GenSpawn.Spawn(ThingMaker.MakeThing(ThingDef.Named("ShardBeacon")), cell, map).SetFaction(faction);
            }
        }

        private void SpawnCorpses(Map map, CellRect rect, Faction faction)
        {
            // Guaranteed sacrifice in the middle
            Pawn centerPawn = CreateVictim(map, faction);
            GenSpawn.Spawn(centerPawn, rect.CenterCell, map);
            KillVictim(centerPawn);

            // Additional corpses around the ritual
            int extra = Rand.RangeInclusive(1, 6);
            List<IntVec3> cells = new List<IntVec3>();
            cells.Add(rect.CenterCell);
            for (int i = 0; i < extra; i++)
            {
                IntVec3 cell = CellFinder.RandomClosewalkCellNear(rect.CenterCell, map, (rect.Width - 1) / 2);

                if (!cell.Standable(map))
                {
                    continue;
                }

                foreach (IntVec3 usedCell in cells)
                {
                    if (cell.AdjacentTo(usedCell) || cell == usedCell || cell == rect.CenterCell + (IntVec3.North * 2))
                    {
                        continue;
                    }
                }

                cells.Add(cell);
                Pawn pawn = CreateVictim(map, faction);
                GenSpawn.Spawn(pawn, cell, map);
                KillVictim(pawn);
            }
        }


        private void SpawnBlood(Map map, CellRect rect)
        {
            foreach (IntVec3 cell in rect.Cells)
            {
                if (Rand.Chance(0.2f))
                {
                    FilthMaker.TryMakeFilth(cell, map, ThingDefOf.Filth_Blood, Rand.Range(1, 4));
                }
            }
        }

        private void SpawnGibbet(Map map, IntVec3 cell, Faction faction)
        {
            ThingDef thingDef = ThingDef.Named("GibbetCage");
            Building_Casket gibbet = (Building_Casket)ThingMaker.MakeThing(thingDef, ThingDefOf.Steel);
            gibbet.SetFaction(faction);

            Pawn pawn = CreateVictim(map, faction);
            KillVictim(pawn);
            gibbet.TryAcceptThing(pawn.Corpse);

            GenSpawn.Spawn(gibbet, cell, map);
        }

        private void SpawnExteriorDefenses(Map map, CellRect rect, Faction faction)
        {
            // Top wall
            for (int x = rect.minX; x <= rect.maxX; x++)
            {
                if (Mathf.Abs(x - rect.CenterCell.x) <= 1)
                    continue;

                GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Sandbags, ThingDefOf.Cloth), new IntVec3(x, 0, rect.maxZ), map).SetFaction(faction);
            }

            // Bottom wall
            for (int x = rect.minX; x <= rect.maxX; x++)
            {
                if (Mathf.Abs(x - rect.CenterCell.x) <= 1)
                    continue;

                GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Sandbags, ThingDefOf.Cloth), new IntVec3(x, 0, rect.minZ), map).SetFaction(faction);
            }

            // Left wall
            for (int z = rect.minZ + 1; z < rect.maxZ; z++)
            {
                if (Mathf.Abs(z - rect.CenterCell.z) <= 1)
                    continue;

                GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Sandbags, ThingDefOf.Cloth), new IntVec3(rect.minX, 0, z), map).SetFaction(faction);
            }

            // Right wall
            for (int z = rect.minZ + 1; z < rect.maxZ; z++)
            {
                if (Mathf.Abs(z - rect.CenterCell.z) <= 1)
                    continue;

                GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Sandbags, ThingDefOf.Cloth), new IntVec3(rect.maxX, 0, z), map).SetFaction(faction);
            }

            // Braziers
            GenSpawn.Spawn(StyledThing(ThingDef.Named("TorchLamp"), faction), new IntVec3(rect.minX - 1, 0, rect.minZ - 1), map);
            GenSpawn.Spawn(StyledThing(ThingDef.Named("TorchLamp"), faction), new IntVec3(rect.maxX + 1, 0, rect.minZ - 1), map);
            GenSpawn.Spawn(StyledThing(ThingDef.Named("TorchLamp"), faction), new IntVec3(rect.minX - 1, 0, rect.maxZ + 1), map);
            GenSpawn.Spawn(StyledThing(ThingDef.Named("TorchLamp"), faction), new IntVec3(rect.maxX + 1, 0, rect.maxZ + 1), map);

            if (ModsConfig.IdeologyActive)
            {
                // Skullspikes
                GenSpawn.Spawn(ThingMaker.MakeThing(ThingDef.Named("Skullspike"), ThingDefOf.Steel), new IntVec3(rect.CenterCell.x - 2, 0, rect.maxZ + 1), map).SetFaction(faction);
                GenSpawn.Spawn(ThingMaker.MakeThing(ThingDef.Named("Skullspike"), ThingDefOf.Steel), new IntVec3(rect.CenterCell.x + 2, 0, rect.maxZ + 1), map).SetFaction(faction);
                GenSpawn.Spawn(ThingMaker.MakeThing(ThingDef.Named("Skullspike"), ThingDefOf.Steel), new IntVec3(rect.CenterCell.x - 2, 0, rect.minZ - 1), map).SetFaction(faction);
                GenSpawn.Spawn(ThingMaker.MakeThing(ThingDef.Named("Skullspike"), ThingDefOf.Steel), new IntVec3(rect.CenterCell.x + 2, 0, rect.minZ - 1), map).SetFaction(faction);

                // Gibbet cages
                SpawnGibbet(map, new IntVec3(rect.minX, 0, rect.CenterCell.z), faction);
                SpawnGibbet(map, new IntVec3(rect.maxX, 0, rect.CenterCell.z), faction);
            }
        }

        private void SpawnCultists(Map map, SitePart sitePart, CellRect ritualRect, Faction faction, CompCauseGameCondition_Ritual compCauser)
        {
            float points = sitePart.parms.threatPoints;

            PawnGroupMakerParms pawnParms = new PawnGroupMakerParms
            {
                tile = map.Tile,
                faction = faction,
                points = Math.Min(points, 750),
                inhabitants = true,
                seed = OutpostSitePartUtility.GetPawnGroupMakerSeed(sitePart.parms),
                groupKind = PawnGroupKindDefOf.Settlement
            };

            List<Pawn> pawns = PawnGroupMakerUtility.GeneratePawns(pawnParms).ToList();

            ritualLeader = pawns.Where(pawn => !pawn.IsMutant).OrderByDescending(pawn => pawn.kindDef.combatPower).First();
            if (ritualLeader == null)
            {
                ritualLeader = pawns.OrderByDescending(pawn => pawn.kindDef.combatPower).First();
            }

            foreach (Pawn pawn in pawns)
            {
                IntVec3 spawnCell = CellFinder.RandomClosewalkCellNear(ritualRect.CenterCell, map, 8);

                if (pawn == ritualLeader)
                {
                    if (ritualLeader.health.hediffSet.HasHediff(HediffDefOf.DeathRefusal) && faction.leader != ritualLeader)
                    {
                        ritualLeader.health.RemoveHediff(ritualLeader.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DeathRefusal));
                    }
                    if (ritualLeader.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamed("BattIeBear_VoidMartyr")))
                    {
                        ritualLeader.health.RemoveHediff(ritualLeader.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("BattIeBear_VoidMartyr")));
                    }
                    spawnCell = ritualRect.CenterCell + (IntVec3.North * 2);
                }

                GenSpawn.Spawn(pawn, spawnCell, map);
            }

            LordMaker.MakeNewLord(faction, new LordJob_ConditionCauser_Ritual(ritualLeader, ritualRect.CenterCell, compCauser), map, pawns);
        }
    }
}