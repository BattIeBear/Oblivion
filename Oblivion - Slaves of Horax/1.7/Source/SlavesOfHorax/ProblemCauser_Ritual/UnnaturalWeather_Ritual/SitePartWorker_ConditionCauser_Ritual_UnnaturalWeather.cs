using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace Oblivion_SlavesOfHorax
{
    public class SitePartWorker_ConditionCauser_Ritual_UnnaturalWeather : SitePartWorker_ConditionCauser_Ritual
    {
        public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
        {
            base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
            WeatherDef weather = part.conditionCauser.TryGetComp<CompCauseGameCondition_Ritual>().weather;
            outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("weather", weather));
        }
    }
}
