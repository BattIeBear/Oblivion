using UnityEngine;
using Verse;

namespace Oblivion_SlavesOfHorax
{
    public class OblivionSlavesOfHoraxSettingsWindow : Mod
    {
        /// A reference to our settings.
        OblivionSlavesOfHoraxSettings settings;

        /// A mandatory constructor which resolves the reference to our settings.
        public OblivionSlavesOfHoraxSettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<OblivionSlavesOfHoraxSettings>();
        }

        /// The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("Oblivion_SlavesOfHorax_TopWarning".Translate());

            listingStandard.GapLine();

            listingStandard.Label("Oblivion_SlavesOfHorax_Label1".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_HoraxShowOnMap".Translate(), ref OblivionSlavesOfHoraxSettings.HoraxShowOnMap, "Oblivion_SlavesOfHorax_HoraxShowOnMapDesc".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_HoraxWillDoRaids".Translate(), ref OblivionSlavesOfHoraxSettings.HoraxWillDoRaids, "Oblivion_SlavesOfHorax_HoraxWillDoRaidsDesc".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_HoraxOnlyBioferrite".Translate(), ref OblivionSlavesOfHoraxSettings.HoraxOnlyBioferrite, "Oblivion_SlavesOfHorax_HoraxOnlyBioferriteDesc".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_HoraxUseGhouls".Translate(), ref OblivionSlavesOfHoraxSettings.HoraxUseGhouls, "Oblivion_SlavesOfHorax_HoraxUseGhoulsDesc".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_HoraxBloodfeeders".Translate(), ref OblivionSlavesOfHoraxSettings.HoraxBloodfeeders, "Oblivion_SlavesOfHorax_HoraxBloodfeedersDesc".Translate());

            listingStandard.GapLine();

            listingStandard.Label("Oblivion_SlavesOfHorax_Label2".Translate());
            //dropdown menu for allowed ritual types
            if (listingStandard.ButtonTextLabeled("Oblivion_SlavesOfHorax_RitualTypes".Translate(), OblivionSlavesOfHoraxSettings.AllowedRitualType.ToString().Translate(), tooltip: "Oblivion_SlavesOfHorax_RitualTypesDesc".Translate()))
            {
                var options = new List<FloatMenuOption>();

                foreach (OblivionSlavesOfHoraxSettings.AllowedRitualTypes ritual in System.Enum.GetValues(typeof(OblivionSlavesOfHoraxSettings.AllowedRitualTypes)))
                {
                    OblivionSlavesOfHoraxSettings.AllowedRitualTypes newRitual = ritual;

                    options.Add(new FloatMenuOption(newRitual.ToString().Translate(),
                        () =>
                        {
                            OblivionSlavesOfHoraxSettings.AllowedRitualType = newRitual;
                        }
                        ));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }
            //dropdown menu for ritual flee states
            if (listingStandard.ButtonTextLabeled("Oblivion_SlavesOfHorax_RitualFleeStates".Translate(), OblivionSlavesOfHoraxSettings.RitualFleeState.ToString().Translate(), tooltip: "Oblivion_SlavesOfHorax_RitualFleeStatesDesc".Translate()))
            {
                var options = new List<FloatMenuOption>();

                foreach (OblivionSlavesOfHoraxSettings.RitualFleeStates state in System.Enum.GetValues(typeof(OblivionSlavesOfHoraxSettings.RitualFleeStates)))
                {
                    OblivionSlavesOfHoraxSettings.RitualFleeStates newState = state;

                    options.Add(new FloatMenuOption(newState.ToString().Translate(),
                        () =>
                        {
                            OblivionSlavesOfHoraxSettings.RitualFleeState = newState;
                        }
                        ));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }

            listingStandard.GapLine();

            listingStandard.Label("Oblivion_SlavesOfHorax_Label3".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_DoRituals".Translate(), ref OblivionSlavesOfHoraxSettings.DoOffMapRituals, "Oblivion_SlavesOfHorax_DoRitualsDesc".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_AllowSunFracture".Translate(), ref OblivionSlavesOfHoraxSettings.AllowSunFracture, "Oblivion_SlavesOfHorax_AllowSunFractureDesc".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_AllowMindFracture".Translate(), ref OblivionSlavesOfHoraxSettings.AllowMindFracture, "Oblivion_SlavesOfHorax_AllowMindFractureDesc".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_AllowGrayPall".Translate(), ref OblivionSlavesOfHoraxSettings.AllowGrayPall, "Oblivion_SlavesOfHorax_AllowGrayPallDesc".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_AllowDeathPall".Translate(), ref OblivionSlavesOfHoraxSettings.AllowDeathPall, "Oblivion_SlavesOfHorax_AllowDeathPallDesc".Translate());
            listingStandard.CheckboxLabeled("Oblivion_SlavesOfHorax_AllowBlindFog".Translate(), ref OblivionSlavesOfHoraxSettings.AllowBlindFog, "Oblivion_SlavesOfHorax_AllowBlindFogDesc".Translate());

            listingStandard.GapLine();

            if (ModsConfig.IsActive("metalocif.cultbuffs"))
            {
                listingStandard.Label("Oblivion_SlavesOfHorax_Label4".Translate());

                float chance = OblivionSlavesOfHoraxSettings.CultBuffChance;
                OblivionSlavesOfHoraxSettings.CultBuffChance = Mathf.RoundToInt(listingStandard.SliderLabeled("Oblivion_SlavesOfHorax_BuffChance".Translate() + Mathf.RoundToInt(OblivionSlavesOfHoraxSettings.CultBuffChance * 100) + "%", OblivionSlavesOfHoraxSettings.CultBuffChance, 0.0f, 1.0f, tooltip: "Oblivion_SlavesOfHorax_BuffChanceDesc".Translate()) * 100) / 100f;
                if (chance != OblivionSlavesOfHoraxSettings.CultBuffChance)
                {
                    CultBuffCompatibility.ApplySettings();
                }
                listingStandard.GapLine();
            }

            listingStandard.Label("Oblivion_SlavesOfHorax_BottomWarning".Translate());
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "Oblivion_SlavesOfHorax_ModName".Translate();
        }
    }
}