using HarmonyLib;
using System.Reflection;
using Verse;

namespace BioSynth_SlavesOfHorax
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.BioSynth_SlavesOfHorax");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
