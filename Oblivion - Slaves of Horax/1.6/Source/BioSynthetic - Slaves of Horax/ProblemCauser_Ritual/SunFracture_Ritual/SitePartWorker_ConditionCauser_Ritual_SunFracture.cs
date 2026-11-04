using RimWorld;
using Verse;

namespace BioSynth_SlavesOfHorax
{
    public class SitePartWorker_ConditionCauser_Ritual_SunFracture : SitePartWorker_ConditionCauser_Ritual
    {
        public override bool IsAvailable()
        {
            if (ModsConfig.IdeologyActive)
            {
                foreach (Map map in Find.Maps)
                {
                    foreach (Pawn freeColonistsAndPrisoner in map.mapPawns.FreeColonistsAndPrisoners)
                    {
                        if ((freeColonistsAndPrisoner.IsFreeNonSlaveColonist && !freeColonistsAndPrisoner.IsPrisonerOfColony) && !freeColonistsAndPrisoner.IsQuestLodger())
                        {
                            Ideo ideo = freeColonistsAndPrisoner.Ideo;
                            if (ideo != null && (ideo.HasMeme(MemeDefOf.Darkness) || ideo.HasMeme(MemeDefOf.Tunneler)))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return base.IsAvailable();
        }
    }
}