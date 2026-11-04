using Verse;

namespace Oblivion_SlavesOfHorax
{
    public class OblivionSlavesOfHoraxSettings : ModSettings
    {
        public enum RitualFleeStates { Oblivion_RitualFleeState_Never, Oblivion_RitualFleeState_Always, Oblivion_RitualFleeState_AbductionOnly }
        public enum AllowedRitualTypes { Oblivion_AllowedRitualType_All, Oblivion_AllowedRitualType_NoShamblers, Oblivion_AllowedRitualType_NoFleshbeasts, Oblivion_AllowedRitualType_AbductionOnly }

        public static bool HoraxShowOnMap = true;
        public static bool HoraxWillDoRaids = true;
        public static bool HoraxOnlyBioferrite = true;
        public static bool HoraxUseGhouls = true;
        public static bool HoraxBloodfeeders = true;
        public static AllowedRitualTypes AllowedRitualType = AllowedRitualTypes.Oblivion_AllowedRitualType_All;
        public static RitualFleeStates RitualFleeState = RitualFleeStates.Oblivion_RitualFleeState_AbductionOnly;
        public static bool DoOffMapRituals = true;
        public static bool AllowSunFracture = true;
        public static bool AllowMindFracture = true;
        public static bool AllowGrayPall = true;
        public static bool AllowDeathPall = true;
        public static bool AllowBlindFog = true;
        public static float CultBuffChance = .10f;

        public static bool DoOffMapRitualsAbsolute
        {
            get
            {
                return DoOffMapRituals && (AllowSunFracture || AllowMindFracture || AllowGrayPall || AllowDeathPall || AllowBlindFog);
            }
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref HoraxShowOnMap, "HoraxShowOnMap", true);
            Scribe_Values.Look(ref HoraxWillDoRaids, "HoraxWillDoRaids", true);
            Scribe_Values.Look(ref HoraxOnlyBioferrite, "HoraxOnlyBioferrite", true);
            Scribe_Values.Look(ref HoraxUseGhouls, "HoraxUseGhouls", true);
            Scribe_Values.Look(ref HoraxBloodfeeders, "HoraxBloodfeeders", true);
            Scribe_Values.Look(ref AllowedRitualType, "AllowedRitualType", AllowedRitualTypes.Oblivion_AllowedRitualType_All);
            Scribe_Values.Look(ref RitualFleeState, "RitualFleeState", RitualFleeStates.Oblivion_RitualFleeState_AbductionOnly);
            Scribe_Values.Look(ref DoOffMapRituals, "DoOffMapRituals", true);
            Scribe_Values.Look(ref AllowSunFracture, "AllowSunFracture", true);
            Scribe_Values.Look(ref AllowMindFracture, "AllowMindFracture", true);
            Scribe_Values.Look(ref AllowGrayPall, "AllowGrayPall", true);
            Scribe_Values.Look(ref AllowDeathPall, "AllowDeathPall", true);
            Scribe_Values.Look(ref AllowBlindFog, "AllowBlindFog", true);
            Scribe_Values.Look(ref CultBuffChance, "CultBuffChance", .10f);
            base.ExposeData();
        }
    }
}