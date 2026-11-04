using HarmonyLib;
using System.Reflection;
using Verse;

namespace Oblivion_SlavesOfHorax
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.Oblivion_SlavesOfHorax");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}