using RimWorld;
using Verse;

namespace Oblivion_SlavesOfHorax
{
    public class GameCondition_PsychicEmanation_Ritual : GameCondition
    {
        public PsychicDroneLevel level = PsychicDroneLevel.BadLow;

        public override string Label
        {
            get
            {
                return def.label + ": " + level.GetLabel().CapitalizeFirst();
            }
        }

        public override string Description => base.Description.Formatted(level.GetLabel().ToLower());

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref level, "level", PsychicDroneLevel.None);
        }
    }
}