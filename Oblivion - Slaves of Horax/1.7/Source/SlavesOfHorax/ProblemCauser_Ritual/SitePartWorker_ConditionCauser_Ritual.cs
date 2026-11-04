using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace Oblivion_SlavesOfHorax
{
    public class SitePartWorker_ConditionCauser_Ritual : SitePartWorker_ConditionCauser
    {
        /// Needed?
        public static readonly SimpleCurve ThreatPointsLootMarketValue = new SimpleCurve
        {
            new CurvePoint(100f, 200f),
            new CurvePoint(250f, 450f),
            new CurvePoint(800f, 1000f),
            new CurvePoint(10000f, 2000f)
        };

        public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
        {
            string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
            lookTargets = new LookTargets(map.Parent);
            return arrivedLetterPart;
        }

        public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
        {
            base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
            int cultistCount = GetCultistsCount(part.site, part.parms);
            outExtraDescriptionRules.Add(new Rule_String("problemCauserLabel", part.def.label ?? string.Empty));
            outExtraDescriptionRules.Add(new Rule_String("cultist_desc", cultistCount == 1 ? "(*Threat)cultist(/Threat) has arrived" : "group of (*Threat)" + cultistCount.ToString() + " cultists(/Threat) has gathered"));
            outExtraDescriptionRules.Add(new Rule_String("problemCauserDescription", part.def.description ?? string.Empty));
        }

        public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
        {
            if (site.Faction.IsPlayer)
            {
                return null;
            }
            return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(GetCultistsCount(site, sitePart.parms), "Cultists");
        }

        public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
        {
            SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
            sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Settlement));
            ///Needed?
            sitePartParams.lootMarketValue = ThreatPointsLootMarketValue.Evaluate(sitePartParams.threatPoints);
            return sitePartParams;
        }

        protected virtual int GetCultistsCount(Site site, SitePartParams parms)
        {
            return PawnGroupMakerUtility.GeneratePawnKindsExample(new PawnGroupMakerParms
            {
                tile = site.Tile,
                faction = site.Faction,
                points = Math.Min(parms.threatPoints, 750),
                inhabitants = true,
                seed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms),
                groupKind = PawnGroupKindDefOf.Settlement
            }).Count();
        }
    }
}