using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BioSynth_SlavesOfHorax
{
    public class GameCondition_ForceWeather_SightlessFog : GameCondition_ForceWeather_Unnatural
    {
        private static readonly IntRange SightstealerIntervalRange = new IntRange(30000, 120000);
        private Dictionary<int, int> nextSightstealerTicks = new Dictionary<int, int>();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref nextSightstealerTicks, "nextSightstealerTicks", LookMode.Value, LookMode.Value);
        }
        public override void Init()
        {
            if (!ModLister.CheckAnomaly("Sightstealer"))
            {
                End();
                return;
            }
            base.Init();
        }

        public override void GameConditionTick()
        {
            int currentTick = Find.TickManager.TicksGame;
            if (currentTick % 60 != 0)
            {
                return;
            }
            foreach (Map affectedMap in AffectedMaps)
            {
                if(nextSightstealerTicks.ContainsKey(affectedMap.uniqueID) == false)
                {
                    nextSightstealerTicks.Add(affectedMap.uniqueID, currentTick + (SightstealerIntervalRange.RandomInRange / Rand.RangeInclusive(1, 4)));
                    continue;
                }

                if (currentTick < nextSightstealerTicks[affectedMap.uniqueID])
                {
                    continue;
                }

                IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, affectedMap);
                parms.forced = true;
                parms.faction = Faction.OfHoraxCult;
                IncidentDef incident = DefDatabase<IncidentDef>.GetNamed("SightstealerArrival");
                incident.Worker.TryExecute(parms);
                nextSightstealerTicks[affectedMap.uniqueID] = currentTick + SightstealerIntervalRange.RandomInRange;
            }
        }
    }
}
