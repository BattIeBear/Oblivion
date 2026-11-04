using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace BioSynth_SlavesOfHorax
{
    public class PsychicRitualToil_PsychicRitualDef_BattIeBear_SummonShamblers_AI : PsychicRitualToil
    {
        private PsychicRitualRoleDef invokerRole;

        private static List<Pawn> tmpPawns = new List<Pawn>();

        protected PsychicRitualToil_PsychicRitualDef_BattIeBear_SummonShamblers_AI()
        {
        }

        public PsychicRitualToil_PsychicRitualDef_BattIeBear_SummonShamblers_AI(PsychicRitualRoleDef invokerRole)
        {
            this.invokerRole = invokerRole;
        }

        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            base.Start(psychicRitual, parent);
            ApplyOutcome(psychicRitual);
        }

        private void ApplyOutcome(PsychicRitual psychicRitual)
        {
            psychicRitual.def.CalculateMaxPower(psychicRitual.assignments, null, out var power);

            float num = (psychicRitual.power = ((PsychicRitualDef_BattIeBear_SummonShamblers_AI)psychicRitual.def).shamblersPointsFromThreatPointsCurve.Evaluate(power));
            int num2 = 0;
            List<Pawn> list = new List<Pawn>();

            PawnKindDef pawnKindDef = PawnKindDefOf.BattIeBear_HoraxianShambler;
            
            while (num > 0f)
            {
                num -= pawnKindDef.combatPower;
                Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, Faction.OfHoraxCult);
                if (pawn.health.hediffSet.HasHediff(HediffDefOf.DeathRefusal))
                {
                    pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DeathRefusal));
                }
                list.Add(pawn);
                num2++;
                if (num2 > 100)
                {
                    break;
                }
            }
            

            if (list.Count > 0 && !psychicRitual.def.letterAICompleteLabel.NullOrEmpty() && !psychicRitual.def.letterAICompleteText.NullOrEmpty())
            {
                Map map = psychicRitual.Map;

                IntVec3 entryCell;

                if (!RCellFinder.TryFindRandomPawnEntryCell(out entryCell,map,CellFinder.EdgeRoadChance_Ignore))
                {
                    entryCell = CellFinder.RandomEdgeCell(map);
                }
                foreach (Pawn pawn in list)
                {
                    IntVec3 spawnCell = CellFinder.RandomClosewalkCellNear(entryCell, map, 8);

                    GenSpawn.Spawn(pawn, spawnCell, map);
                    //This works, but do I want it to?
                    pawn.SetFaction(Faction.OfHoraxCult);
                }
                LordMaker.MakeNewLord(Faction.OfHoraxCult, new LordJob_AssaultColony(Faction.OfHoraxCult, canKidnap: false, canTimeoutOrFlee: false), map, list);
                Find.LetterStack.ReceiveLetter(psychicRitual.def.letterAICompleteLabel, psychicRitual.def.letterAICompleteText, LetterDefOf.ThreatBig, list);
            }
        }

        public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            foreach (Pawn allAssignedPawn in psychicRitual.assignments.AllAssignedPawns)
            {
                SetPawnDuty(allAssignedPawn, psychicRitual, parent, DutyDefOf.Idle);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
        }
    }
}