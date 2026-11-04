using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BioSynth_SlavesOfHorax
{
    public class BioSynthSlavesOfHoraxSettingsWindow : Mod
    {
        /// A reference to our settings.
        BioSynthSlavesOfHoraxSettings settings;

        /// A mandatory constructor which resolves the reference to our settings.
        public BioSynthSlavesOfHoraxSettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BioSynthSlavesOfHoraxSettings>();
        }

        /// The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("BioSynth_SlavesOfHorax_TopWarning".Translate());

            listingStandard.GapLine();

            listingStandard.Label("BioSynth_SlavesOfHorax_Label1".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_HoraxShowOnMap".Translate(), ref BioSynthSlavesOfHoraxSettings.HoraxShowOnMap, "BioSynth_SlavesOfHorax_HoraxShowOnMapDesc".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_HoraxWillDoRaids".Translate(), ref BioSynthSlavesOfHoraxSettings.HoraxWillDoRaids, "BioSynth_SlavesOfHorax_HoraxWillDoRaidsDesc".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_HoraxOnlyBioferrite".Translate(), ref BioSynthSlavesOfHoraxSettings.HoraxOnlyBioferrite, "BioSynth_SlavesOfHorax_HoraxOnlyBioferriteDesc".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_HoraxUseGhouls".Translate(), ref BioSynthSlavesOfHoraxSettings.HoraxUseGhouls, "BioSynth_SlavesOfHorax_HoraxUseGhoulsDesc".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_HoraxBloodfeeders".Translate(), ref BioSynthSlavesOfHoraxSettings.HoraxBloodfeeders, "BioSynth_SlavesOfHorax_HoraxBloodfeedersDesc".Translate());

            listingStandard.GapLine();

            listingStandard.Label("BioSynth_SlavesOfHorax_Label2".Translate());
            //dropdown menu for allowed ritual types
            if (listingStandard.ButtonTextLabeled("BioSynth_SlavesOfHorax_RitualTypes".Translate(), BioSynthSlavesOfHoraxSettings.AllowedRitualType.ToString().Translate(), tooltip: "BioSynth_SlavesOfHorax_RitualTypesDesc".Translate()))
            {
                var options = new List<FloatMenuOption>();

                foreach (BioSynthSlavesOfHoraxSettings.AllowedRitualTypes ritual in System.Enum.GetValues(typeof(BioSynthSlavesOfHoraxSettings.AllowedRitualTypes)))
                {
                    BioSynthSlavesOfHoraxSettings.AllowedRitualTypes newRitual = ritual;

                    options.Add(new FloatMenuOption(newRitual.ToString().Translate(),
                        () =>
                        {
                            BioSynthSlavesOfHoraxSettings.AllowedRitualType = newRitual;
                        }
                        ));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }
            //dropdown menu for ritual flee states
            if (listingStandard.ButtonTextLabeled("BioSynth_SlavesOfHorax_RitualFleeStates".Translate(), BioSynthSlavesOfHoraxSettings.RitualFleeState.ToString().Translate(), tooltip: "BioSynth_SlavesOfHorax_RitualFleeStatesDesc".Translate()))
            {
                var options = new List<FloatMenuOption>();

                foreach (BioSynthSlavesOfHoraxSettings.RitualFleeStates state in System.Enum.GetValues(typeof(BioSynthSlavesOfHoraxSettings.RitualFleeStates)))
                {
                    BioSynthSlavesOfHoraxSettings.RitualFleeStates newState = state;

                    options.Add(new FloatMenuOption(newState.ToString().Translate(),
                        () =>
                        {
                            BioSynthSlavesOfHoraxSettings.RitualFleeState = newState;
                        }
                        ));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }

            listingStandard.GapLine();

            listingStandard.Label("BioSynth_SlavesOfHorax_Label3".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_DoRituals".Translate(), ref BioSynthSlavesOfHoraxSettings.DoOffMapRituals, "BioSynth_SlavesOfHorax_DoRitualsDesc".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_AllowSunFracture".Translate(), ref BioSynthSlavesOfHoraxSettings.AllowSunFracture, "BioSynth_SlavesOfHorax_AllowSunFractureDesc".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_AllowMindFracture".Translate(), ref BioSynthSlavesOfHoraxSettings.AllowMindFracture, "BioSynth_SlavesOfHorax_AllowMindFractureDesc".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_AllowGrayPall".Translate(), ref BioSynthSlavesOfHoraxSettings.AllowGrayPall, "BioSynth_SlavesOfHorax_AllowGrayPallDesc".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_AllowDeathPall".Translate(), ref BioSynthSlavesOfHoraxSettings.AllowDeathPall, "BioSynth_SlavesOfHorax_AllowDeathPallDesc".Translate());
            listingStandard.CheckboxLabeled("BioSynth_SlavesOfHorax_AllowBlindFog".Translate(), ref BioSynthSlavesOfHoraxSettings.AllowBlindFog, "BioSynth_SlavesOfHorax_AllowBlindFogDesc".Translate());

            listingStandard.GapLine();

            if (ModsConfig.IsActive("metalocif.cultbuffs"))
            {
                listingStandard.Label("BioSynth_SlavesOfHorax_Label4".Translate());

                float chance = BioSynthSlavesOfHoraxSettings.CultBuffChance;
                BioSynthSlavesOfHoraxSettings.CultBuffChance = Mathf.RoundToInt(listingStandard.SliderLabeled("BioSynth_SlavesOfHorax_BuffChance".Translate() + Mathf.RoundToInt(BioSynthSlavesOfHoraxSettings.CultBuffChance * 100) + "%", BioSynthSlavesOfHoraxSettings.CultBuffChance, 0.0f, 1.0f, tooltip: "BioSynth_SlavesOfHorax_BuffChanceDesc".Translate()) * 100) / 100f;
                if(chance != BioSynthSlavesOfHoraxSettings.CultBuffChance)
                {
                    CultBuffCompatibility.ApplySettings();
                }
                listingStandard.GapLine();
            }

            listingStandard.Label("BioSynth_SlavesOfHorax_BottomWarning".Translate());
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "BioSynth_SlavesOfHorax_ModName".Translate();
        }
    }
}