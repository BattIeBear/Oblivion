using RimWorld;
using RimWorld.Planet;
using System;
using Verse;

namespace BioSynth_SlavesOfHorax
{
    public class ThoughtWorker_MindFracture_Ritual : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            PsychicDroneLevel psychicDroneLevel = PsychicDroneLevel.None;
            Map mapHeld = p.MapHeld;
            if (p.Faction == Faction.OfHoraxCult)
            {
                return false;
            }
            if (mapHeld != null)
            {
                GameCondition_PsychicEmanation_Ritual activeCondition = mapHeld.gameConditionManager.GetActiveCondition<GameCondition_PsychicEmanation_Ritual>();
                if (activeCondition == null)
                {
                    return false;
                }
                if ((int)activeCondition.level > (int)psychicDroneLevel)
                {
                    psychicDroneLevel = activeCondition.level;
                }
            }
            else if (p.IsCaravanMember())
            {
                foreach (Site site in Find.World.worldObjects.Sites)
                {
                    foreach (SitePart part in site.parts)
                    {
                        if (!part.conditionCauser.DestroyedOrNull() && part.def.Worker is SitePartWorker_ConditionCauser_PsychicDroner)
                        {
                            CompCauseGameCondition_Ritual compCauseGameCondition_Ritual = part.conditionCauser.TryGetComp<CompCauseGameCondition_Ritual>();
                            if (compCauseGameCondition_Ritual.ConditionDef.conditionClass == typeof(GameCondition_PsychicEmanation_Ritual) && compCauseGameCondition_Ritual.InAoE(p.GetCaravan().Tile) && (int)compCauseGameCondition_Ritual.droneLevel > (int)psychicDroneLevel)
                            {
                                psychicDroneLevel = compCauseGameCondition_Ritual.droneLevel;
                            }
                        }
                    }
                }
                foreach (Map map in Find.Maps)
                {
                    foreach (GameCondition activeCondition in map.gameConditionManager.ActiveConditions)
                    {
                        CompCauseGameCondition_Ritual compCauseGameCondition_Ritual2 = activeCondition.conditionCauser.TryGetComp<CompCauseGameCondition_Ritual>();
                        if (compCauseGameCondition_Ritual2 != null && compCauseGameCondition_Ritual2.InAoE(p.GetCaravan().Tile) && (int)compCauseGameCondition_Ritual2.droneLevel > (int)psychicDroneLevel)
                        {
                            psychicDroneLevel = compCauseGameCondition_Ritual2.droneLevel;
                        }
                    }
                }
            }
            return psychicDroneLevel switch
            {
                PsychicDroneLevel.None => false,
                PsychicDroneLevel.GoodMedium => ThoughtState.ActiveAtStage(0),
                PsychicDroneLevel.BadLow => ThoughtState.ActiveAtStage(1),
                PsychicDroneLevel.BadMedium => ThoughtState.ActiveAtStage(2),
                PsychicDroneLevel.BadHigh => ThoughtState.ActiveAtStage(3),
                PsychicDroneLevel.BadExtreme => ThoughtState.ActiveAtStage(4),
                _ => throw new NotImplementedException(),
            };
        }
    }
}