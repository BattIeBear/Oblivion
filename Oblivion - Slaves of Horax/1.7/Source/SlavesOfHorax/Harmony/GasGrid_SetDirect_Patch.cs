using HarmonyLib;
using Verse;

namespace Oblivion_SlavesOfHorax
{
    [HarmonyPatch(typeof(GasGrid), nameof(GasGrid.SetDirect), typeof(int), typeof(byte), typeof(byte), typeof(byte), typeof(byte))]
    public static class GasGrid_SetDirect_Patch
    {
        static HashSet<int> recentCells = new HashSet<int>();
        public static void Postfix(GasGrid __instance, int index, byte smoke, byte toxic, byte rotStink, byte deadlife)
        {
            if (deadlife == 0 && (smoke + toxic + rotStink) != 0)
            {
                return;
            }

            if (recentCells.Contains(index) && deadlife != 0)
            {
                return;
            }

            Map map = Traverse.Create(__instance).Field("map").GetValue<Map>();

            if (map == null)
            {
                return;
            }

            HoraxGasMapComponenet horaxGas = map.GetHoraxGas();
            if (horaxGas == null || horaxGas.HasAny == false)
            {
                return;
            }

            if (deadlife == 0)
            {
                horaxGas.Remove(CellIndicesUtility.IndexToCell(index, map.Size.x));
            }
            else
            {
                horaxGas.TryToSpread(CellIndicesUtility.IndexToCell(index, map.Size.x));
                recentCells.Add(index);
            }
        }
        public static void Clear()
        {
            recentCells.Clear();
        }
    }
}
