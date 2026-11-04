using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace Oblivion_SlavesOfHorax
{
    [HarmonyPatch(typeof(MutantUtility), nameof(MutantUtility.ResurrectAsShambler))]
    public class MutantUtility_ResurrectAsShambler_Patch
    {
        public static void Postfix(Pawn pawn)
        {
            if (pawn.Map == null)
            {
                return;
            }

            HoraxGasMapComponenet horaxGas = pawn.Map.GetHoraxGas();
            if (horaxGas == null || horaxGas.HasAny == false)
            {
                return;
            }

            if (horaxGas.HasHoraxGas(pawn.Position) && pawn.Faction != Faction.OfHoraxCult)
            {
                pawn.SetFaction(Faction.OfHoraxCult);
                if (pawn.Map.lordManager == null)
                {
                    return;
                }

                if (pawn.Map.lordManager.lords.NullOrEmpty())
                {
                    return;
                }

                foreach (Lord lord in pawn.Map.lordManager.lords)
                {
                    if (lord.faction == Faction.OfHoraxCult)
                    {
                        lord.AddPawn(pawn);
                        return;
                    }
                }
            }
        }
    }
}
