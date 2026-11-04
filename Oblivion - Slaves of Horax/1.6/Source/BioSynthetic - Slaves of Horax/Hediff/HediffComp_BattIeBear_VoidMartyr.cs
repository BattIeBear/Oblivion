using RimWorld;
using Verse;

namespace BioSynth_SlavesOfHorax
{
    public class HediffComp_BattIeBear_VoidMartyr : HediffComp
    {
        public HediffCompProperties_BattIeBear_VoidMartyr Props => (HediffCompProperties_BattIeBear_VoidMartyr)props;
        private int GasAmount = 255 * 5;
        public override void Notify_PawnKilled()
        {
            base.Notify_PawnKilled();

            if (Pawn.Map == null)
            {
                return;
            }

            HoraxGasMapComponenet horaxGas = Pawn.Map.GetHoraxGas();
            if (horaxGas == null)
            {
                return;
            }
            horaxGas.Add(Pawn.Position);
            GasUtility.AddDeadifeGas(Pawn.Position, Pawn.Map, Faction.OfHoraxCult, GasAmount);
        }
    }
}
