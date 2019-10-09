using System.Reflection;
using System.Collections.Generic;
using Verse;
using RimWorld;
using Harmony;

namespace BSL
{
    [StaticConstructorOnStartup]
    public class BSL
    {
        static BSL()
        {            
            var harmony = HarmonyInstance.Create("com.github.toywalrus.bsl");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

    }

    [HarmonyPatch(typeof(Plant))]
    [HarmonyPatch("Resting", MethodType.Getter)]
    public static class Patch
    { 
        [HarmonyPostfix]
        public static void IsResting(Plant __instance, ref bool __result)
        {
            bool isInBasin = false;
            CellRect.CellRectIterator cri = __instance.OccupiedRect().GetIterator();
            while (!cri.Done())
            {
                List<Thing> thingList = __instance.Map.thingGrid.ThingsListAt(cri.Current);
                for (int i = 0; i < thingList.Count; i++)
                {
                    Building_PlantGrower pg = thingList[i] as Building_PlantGrower;
                    if (pg != null)
                    {
                        isInBasin = true;
                        break;
                    }
                }
                cri.MoveNext();
            }
            if (isInBasin) __result = false;
        }
    }
}