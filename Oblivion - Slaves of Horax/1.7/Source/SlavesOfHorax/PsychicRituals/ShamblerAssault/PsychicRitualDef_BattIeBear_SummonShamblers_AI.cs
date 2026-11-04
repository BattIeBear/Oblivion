using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Oblivion_SlavesOfHorax
{
    public class PsychicRitualDef_BattIeBear_SummonShamblers_AI : PsychicRitualDef_InvocationCircle
    {
        public SimpleCurve shamblersPointsFromThreatPointsCurve;

        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new PsychicRitualToil_PsychicRitualDef_BattIeBear_SummonShamblers_AI(InvokerRole));
            return list;
        }

        public override void CalculateMaxPower(PsychicRitualRoleAssignments assignments, List<QualityFactor> powerFactorsOut, out float power)
        {
            power = 0f;
            if (assignments.FirstAssignedPawn(InvokerRole)?.GetLord()?.LordJob is LordJob_PsychicRitual lordJob_PsychicRitual)
            {
                power = Mathf.Max(power, lordJob_PsychicRitual.points);
            }
        }
    }
}
