using HarmonyLib;
using Verse;
using Verse.AI.Group;

namespace BioSynth_SlavesOfHorax
{
    [HarmonyPatch(typeof(Hediff_PsychicTrance), "get_ShouldRemove")]

    public static class Hediff_PsychicTrance_ShouldRemove_Patch
    {
        public static void Postfix(ref bool __result, Hediff_PsychicTrance __instance)
        {
            if (__result == false)
            {
                return;
            }

            Lord lord = __instance.pawn.GetLord();
            if (lord != null && lord.LordJob is LordJob_ConditionCauser_Ritual)
            {
                __result = false;
            }
        }
    }
}
