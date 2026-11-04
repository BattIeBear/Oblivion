using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace BioSynth_SlavesOfHorax
{
    public class LordToil_ConditionCauser_Ritual : LordToil
    {
        public Pawn ritualLeader;
        public IntVec3 ritualCell;

        public LordToil_ConditionCauser_Ritual()
        {
        }

        public LordToil_ConditionCauser_Ritual(Pawn ritualLeader, IntVec3 ritualCell)
        {
            this.ritualLeader = ritualLeader;
            this.ritualCell = ritualCell;
        }

        public override void UpdateAllDuties()
        {
            foreach (Pawn pawn in lord.ownedPawns)
            {
                if (pawn == ritualLeader)
                {
                    pawn.mindState.duty = new PawnDuty(DefDatabase<DutyDef>.GetNamed("BattIeBear_RitualLeader"), ritualCell, 0f);
                }
                else
                {
                    pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, ritualCell, 12f);
                }

                pawn.health.AddHediff(HediffDefOf.PsychicTrance);
            }
        }
    }
}
