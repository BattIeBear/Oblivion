using System.Reflection;
using System.Xml;
using Verse;
using System;

namespace BioSynth_SlavesOfHorax
{
    public class HoraxShowOnMap : PatchOperation
    {
        private PatchOperation Show;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BioSynthSlavesOfHoraxSettings.HoraxShowOnMap)
            {
                Show.Apply(xml);
            }
            return true;
        }
    }
    public class HoraxRaids : PatchOperation
    {
        private PatchOperation Raid;
        private PatchOperation NoRaid;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BioSynthSlavesOfHoraxSettings.HoraxWillDoRaids)
            {
                Raid.Apply(xml);
            }
            else
            {
                NoRaid.Apply(xml);
            }
            return true;
        }
    }
    public class OnlyHoraxUsesBioferrite : PatchOperation
    {
        private PatchOperation HoraxOnly;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BioSynthSlavesOfHoraxSettings.HoraxOnlyBioferrite)
            {
                HoraxOnly.Apply(xml);
            }
            return true;
        }
    }

    public class HoraxUsesGhouls : PatchOperation
    {
        private PatchOperation HoraxGhouls;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BioSynthSlavesOfHoraxSettings.HoraxUseGhouls)
            {
                HoraxGhouls.Apply(xml);
            }
            return true;
        }
    }

    public class HoraxWorshipsBloodfeeders : PatchOperation
    {
        private PatchOperation HoraxBloodfeeders;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BioSynthSlavesOfHoraxSettings.HoraxBloodfeeders)
            {
                HoraxBloodfeeders.Apply(xml);
            }
            return true;
        }
    }

    public class AllowedPsychicRituals : PatchOperation
    {
        private PatchOperation NoShamblers;
        private PatchOperation NoFlesh;
        private PatchOperation Neither;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BioSynthSlavesOfHoraxSettings.AllowedRitualType == BioSynthSlavesOfHoraxSettings.AllowedRitualTypes.Biosynth_AllowedRitualType_NoShamblers)
            {
                NoShamblers.Apply(xml);
            }
            if (BioSynthSlavesOfHoraxSettings.AllowedRitualType == BioSynthSlavesOfHoraxSettings.AllowedRitualTypes.Biosynth_AllowedRitualType_NoFleshbeasts)
            {
                NoFlesh.Apply(xml);
            }
            if (BioSynthSlavesOfHoraxSettings.AllowedRitualType == BioSynthSlavesOfHoraxSettings.AllowedRitualTypes.Biosynth_AllowedRitualType_AbductionOnly)
            {
                Neither.Apply(xml);
            }
            return true;
        }
    }
    
    public class DoOffMapRituals : PatchOperation
    {
        private PatchOperation No;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (!BioSynthSlavesOfHoraxSettings.DoOffMapRitualsAbsolute)
            {
                No.Apply(xml);
            }
            return true;
        }
    }
    
    public class DoSunFracture : PatchOperation
    {
        private PatchOperation No;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (!BioSynthSlavesOfHoraxSettings.AllowSunFracture)
            {
                No.Apply(xml);
            }
            return true;
        }
    }
    public class DoMindFracture : PatchOperation
    {
        private PatchOperation No;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (!BioSynthSlavesOfHoraxSettings.AllowMindFracture)
            {
                No.Apply(xml);
            }
            return true;
        }
    }
    public class DoGrayPall : PatchOperation
    {
        private PatchOperation No;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (!BioSynthSlavesOfHoraxSettings.AllowGrayPall)
            {
                No.Apply(xml);
            }
            return true;
        }
    }
    public class DoDeathPall : PatchOperation
    {
        private PatchOperation No;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (!BioSynthSlavesOfHoraxSettings.AllowDeathPall)
            {
                No.Apply(xml);
            }
            return true;
        }
    }
    public class DoBlindFog : PatchOperation
    {
        private PatchOperation No;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (!BioSynthSlavesOfHoraxSettings.AllowBlindFog)
            {
                No.Apply(xml);
            }
            return true;
        }
    }

    [StaticConstructorOnStartup]
    public static class Startup
    {
        static Startup()
        {
            CultBuffCompatibility.ApplySettings();
        }
    }
    public static class CultBuffCompatibility
    {
        public static void ApplySettings()
        {
            if (!ModsConfig.IsActive("metalocif.cultbuffs"))
            {
                return;
            }

            float chance = BioSynthSlavesOfHoraxSettings.CultBuffChance;

            SetChance("Horaxian_Underthrall", "CultBuffs_FleshBuff", chance);
            SetChance("Horaxian_Highthrall", "CultBuffs_MetalBuff", chance);
            SetChance("BattIeBear_Horaxian_MeleeHighthrall", "CultBuffs_MetalBuff", chance);
            SetChance("BattIeBear_Horaxian_Orator", "CultBuffs_DarknessBuff", chance);
            SetChance("BattIeBear_Horaxian_MouthOfOblivion", "CultBuffs_DarknessBuff", chance);
        }

        private static void SetChance(string pawnKindDefName, string hediffDefName, float chance)
        {
            PawnKindDef pawnKind = DefDatabase<PawnKindDef>.GetNamedSilentFail(pawnKindDefName);
            if (pawnKind == null)
            {
                return;
            }
            if (pawnKind?.startingHediffs == null)
            {
                return;
            }

            foreach (var hediff in pawnKind.startingHediffs)
            {
                if (hediff.def.defName == hediffDefName)
                {
                    hediff.chance = chance;
                    return;
                }
            }
        }
    }
}
