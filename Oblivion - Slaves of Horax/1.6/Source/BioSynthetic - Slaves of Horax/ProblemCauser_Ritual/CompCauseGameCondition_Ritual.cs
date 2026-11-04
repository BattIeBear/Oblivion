using RimWorld;
using Verse;

namespace BioSynth_SlavesOfHorax
{
    public class CompCauseGameCondition_Ritual : CompCauseGameCondition
    {
        public WeatherDef weather = null;
        public PsychicDroneLevel droneLevel = PsychicDroneLevel.None;

        private int ticksToIncreaseDroneLevel;
        private int nextDroneLevelIncreaseInterval
        {
            get
            {
                return Rand.RangeInclusive(120000, 180000);
            }
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            weather = base.Props.conditionDef.weatherDef;
        }

        protected override void SetupCondition(GameCondition condition, Map map)
        {
            base.SetupCondition(condition, map);
            if (weather != null)
            {
                ((GameCondition_ForceWeather_Unnatural)condition).weather = weather;
            }


            if (condition is GameCondition_PsychicEmanation_Ritual psychicCondition)
            {
                PsychicDroneLevel highestLevel = PsychicDroneLevel.None;
                foreach (GameCondition condition2 in CausedConditions)
                {
                    if (condition2 is GameCondition_PsychicEmanation_Ritual psychicCondition2)
                    {
                        if (psychicCondition2.level > highestLevel)
                        {
                            highestLevel = psychicCondition2.level;
                        }
                    }
                }
                psychicCondition.level = highestLevel;
                droneLevel = psychicCondition.level;
                ticksToIncreaseDroneLevel = nextDroneLevelIncreaseInterval;
            }
        }

        public override string CompInspectStringExtra()
        {
            string text = base.CompInspectStringExtra();
            if (weather == null)
            {
                return text;
            }
            if (!text.NullOrEmpty())
            {
                text += "\n";
            }

            string newText = "";

            if (weather != null)
            {
                newText = "Weather".Translate() + ": " + weather.LabelCap;
            }
            if (droneLevel != PsychicDroneLevel.None)
            {
                if (!newText.NullOrEmpty())
                {
                    newText += "\n";
                }
                newText += "PsychicDroneLevel".Translate() + ": " + droneLevel.GetLabel();
            }

            return text + newText;
        }
        public override void CompTick()
        {
            base.CompTick();
            if (Active && droneLevel != PsychicDroneLevel.None && droneLevel != PsychicDroneLevel.BadExtreme)
            {
                ticksToIncreaseDroneLevel--;
                if (ticksToIncreaseDroneLevel <= 0)
                {
                    IncreaseDroneLevel();
                    ticksToIncreaseDroneLevel = nextDroneLevelIncreaseInterval;
                }
            }
        }
        private void IncreaseDroneLevel()
        {
            if (droneLevel != PsychicDroneLevel.BadExtreme)
            {
                droneLevel++;
                foreach (GameCondition condition in CausedConditions)
                {
                    if (condition is GameCondition_PsychicEmanation_Ritual psychicCondition)
                    {
                        psychicCondition.level = droneLevel;
                    }
                }
                TaggedString text = "LetterMindFractureLevelIncreased".Translate();
                Find.LetterStack.ReceiveLetter("LetterLabelMindFractureLevelIncreased".Translate(), text, LetterDefOf.NegativeEvent);
                ReSetupAllConditions();
            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {

        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Defs.Look(ref weather, "weather");
            Scribe_Values.Look(ref droneLevel, "droneLevel", PsychicDroneLevel.None);
            Scribe_Values.Look(ref ticksToIncreaseDroneLevel, "ticksToIncreaseDroneLevel", 0);
        }
    }
}
