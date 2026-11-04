using RimWorld;
using System.Collections.Generic;
using Verse.AI;

namespace BioSynth_SlavesOfHorax
{
    public class JobDriver_ConditionCauser_RitualLeader : JobDriver
    {

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Toil stand = Toils_General.Wait(int.MaxValue);

            stand.tickIntervalAction = delegate (int delta)
            {
                pawn.rotationTracker.FaceCell(pawn.mindState.duty.focus.Cell);
                pawn.GainComfortFromCellIfPossible(delta);
            };

            stand.socialMode = RandomSocialMode.Off;
            stand.defaultCompleteMode = ToilCompleteMode.Never;
            stand.handlingFacing = true;

            yield return stand;
        }
    }
}
