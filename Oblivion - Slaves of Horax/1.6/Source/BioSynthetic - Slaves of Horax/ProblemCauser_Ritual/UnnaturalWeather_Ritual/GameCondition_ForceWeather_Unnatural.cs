using RimWorld;
using Verse;

namespace BioSynth_SlavesOfHorax
{
    public class GameCondition_ForceWeather_Unnatural : GameCondition
    {
        public WeatherDef weather;

        public override int TransitionTicks => 0;

        public override void Init()
        {
            base.Init();
            if (weather == null)
            {
                weather = def.weatherDef;
                foreach (Map map in AffectedMaps)
                {
                    map.weatherDecider.StartNextWeather();
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref weather, "weather");
        }

        public override WeatherDef ForcedWeather()
        {
            return weather;
        }

        public override void End()
        {
            base.End();
            if (weather != null)
            {
                foreach (Map map in AffectedMaps)
                {
                    map.weatherDecider.StartNextWeather();
                }
            }
        }

    }
}
