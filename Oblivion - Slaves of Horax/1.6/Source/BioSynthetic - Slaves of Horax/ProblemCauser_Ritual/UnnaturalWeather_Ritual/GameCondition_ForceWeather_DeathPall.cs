using RimWorld;
using Verse;

namespace BioSynth_SlavesOfHorax
{
    public class GameCondition_ForceWeather_DeathPall : GameCondition_ForceWeather_Unnatural
    {
        private static readonly IntRange ResurrectIntervalRange = new IntRange(600, 1800);

        private int nextResurrectTick;

        private int lastResurrectMessageTick = -9999;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref nextResurrectTick, "nextResurrectTick", 0);
            Scribe_Values.Look(ref lastResurrectMessageTick, "lastResurrectMessageTick", -9999);
        }

        public override void Init()
        {
            if (!ModLister.CheckAnomaly("Death pall"))
            {
                End();
                return;
            }
            base.Init();
            nextResurrectTick = Find.TickManager.TicksGame + ResurrectIntervalRange.RandomInRange;
        }

        public override void GameConditionTick()
        {
            if (Find.TickManager.TicksGame < nextResurrectTick || Find.TickManager.TicksGame % 60 != 0)
            {
                return;
            }
            foreach (Map affectedMap in base.AffectedMaps)
            {
                foreach (Thing item in affectedMap.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
                {
                    if (item is Corpse corpse && MutantUtility.CanResurrectAsShambler(corpse) && corpse.Age >= 15000)
                    {
                        Pawn pawn = ResurrectPawn(corpse);
                        if (!pawn.Position.Fogged(affectedMap))
                        {
                            TryDoMessage(pawn);
                        }
                        nextResurrectTick = Find.TickManager.TicksGame + ResurrectIntervalRange.RandomInRange;
                        return;
                    }
                }
            }
        }

        private Pawn ResurrectPawn(Corpse corpse)
        {
            Pawn innerPawn = corpse.InnerPawn;
            MutantUtility.ResurrectAsShambler(innerPawn, 60000, Faction.OfHoraxCult);
            innerPawn.SetFaction(Faction.OfHoraxCult);
            return innerPawn;
        }

        private void TryDoMessage(LookTargets target)
        {
            if (Find.TickManager.TicksGame - lastResurrectMessageTick >= 60000)
            {
                Messages.Message("DeathPallResurrectedMessage".Translate(), target, MessageTypeDefOf.NegativeEvent, historical: false);
                lastResurrectMessageTick = Find.TickManager.TicksGame;
            }
        }
    }
}
