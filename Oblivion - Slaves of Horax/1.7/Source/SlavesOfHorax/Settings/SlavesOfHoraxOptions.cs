using System.Xml;
using Verse;

namespace Oblivion_SlavesOfHorax
{
    public class HoraxShowOnMap : PatchOperation
    {
        private PatchOperation Show;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (OblivionSlavesOfHoraxSettings.HoraxShowOnMap)
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
            if (OblivionSlavesOfHoraxSettings.HoraxWillDoRaids)
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
            if (OblivionSlavesOfHoraxSettings.HoraxOnlyBioferrite)
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
            if (OblivionSlavesOfHoraxSettings.HoraxUseGhouls)
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
            if (OblivionSlavesOfHoraxSettings.HoraxBloodfeeders)
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
            if (OblivionSlavesOfHoraxSettings.AllowedRitualType == OblivionSlavesOfHoraxSettings.AllowedRitualTypes.Oblivion_AllowedRitualType_NoShamblers)
            {
                NoShamblers.Apply(xml);
            }
            if (OblivionSlavesOfHoraxSettings.AllowedRitualType == OblivionSlavesOfHoraxSettings.AllowedRitualTypes.Oblivion_AllowedRitualType_NoFleshbeasts)
            {
                NoFlesh.Apply(xml);
            }
            if (OblivionSlavesOfHoraxSettings.AllowedRitualType == OblivionSlavesOfHoraxSettings.AllowedRitualTypes.Oblivion_AllowedRitualType_AbductionOnly)
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
            if (!OblivionSlavesOfHoraxSettings.DoOffMapRitualsAbsolute)
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
            if (!OblivionSlavesOfHoraxSettings.AllowSunFracture)
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
            if (!OblivionSlavesOfHoraxSettings.AllowMindFracture)
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
            if (!OblivionSlavesOfHoraxSettings.AllowGrayPall)
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
            if (!OblivionSlavesOfHoraxSettings.AllowDeathPall)
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
            if (!OblivionSlavesOfHoraxSettings.AllowBlindFog)
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

            float chance = OblivionSlavesOfHoraxSettings.CultBuffChance;

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
