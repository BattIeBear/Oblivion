using Verse;
using Verse.AI;

namespace BioSynth_SlavesOfHorax
{
    public class JobGiver_ConditionCauser_RitualLeader : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.mindState?.duty?.def?.defName != "BattIeBear_RitualLeader")
            {
                return null;
            }

            return JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("BattIeBear_RitualLeader"));
        }
    }
}
