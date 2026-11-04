using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace BioSynth_SlavesOfHorax
{
    [HarmonyPatch(typeof(RaidStrategyWorker_PsychicRitualSiege), "MakeLordJob")]
    public static class RaidStrategyWorker_PsychicRitualSiege_LordJob_Prefix
    {
        private static readonly IntRange PsychicRitualIterationsCountRange = new IntRange(3, 5);
        public static bool Prefix(ref LordJob __result, ref IncidentParms parms, ref Map map, ref List<Pawn> pawns, ref int raidSeed)
        {
            if(BioSynthSlavesOfHoraxSettings.RitualFleeState == BioSynthSlavesOfHoraxSettings.RitualFleeStates.Biosynth_RitualFleeState_Never)
            {
                return true;
            }
            if(BioSynthSlavesOfHoraxSettings.RitualFleeState == BioSynthSlavesOfHoraxSettings.RitualFleeStates.Biosynth_RitualFleeState_AbductionOnly && parms.psychicRitualDef.defName != "SkipAbduction")
            {
                return true;
            }
            int iterations = PsychicRitualIterationsCountRange.RandomInRange;
            if (iterations > map.PlayerPawnsForStoryteller.EnumerableCount())
            {
                iterations = map.PlayerPawnsForStoryteller.EnumerableCount();
            }
            Pawn pawn = BestInvoker(pawns);
            PsychicRitualRoleAssignments psychicRitualRoleAssignments = parms.psychicRitualDef.BuildRoleAssignments(new TargetInfo(parms.spawnCenter, map));
            psychicRitualRoleAssignments.AddForcedRole(pawn, PsychicRitualRoleDefOf.Invoker, PsychicRitualRoleDef.Context.NonPlayerFaction);
            for (int i = 0; i < pawns.Count; i++)
            {
                if (pawns[i] != pawn)
                {
                    psychicRitualRoleAssignments.AddForcedRole(pawns[i], PsychicRitualRoleDefOf.Defender, PsychicRitualRoleDef.Context.NonPlayerFaction);
                }
            }
            __result = new LordJob_BattIeBear_PsychicRitualRepeating(parms.psychicRitualDef, psychicRitualRoleAssignments, iterations, parms.spawnCenter, parms.points);

            return false;
        }

        private static Pawn BestInvoker(List<Pawn> pawns)
        {
            Pawn pawn = pawns[0];
            for (int i = 1; i < pawns.Count; i++)
            {
                if (!(pawns[i].GetStatValue(StatDefOf.PsychicSensitivity) <= 0f) && pawns[i].kindDef.isGoodPsychicRitualInvoker && pawns[i].kindDef.combatPower > pawn.kindDef.combatPower)
                {
                    pawn = pawns[i];
                }
            }
            return pawn;
        }
    }
}
