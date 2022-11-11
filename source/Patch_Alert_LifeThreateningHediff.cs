using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;

namespace NoHemogenFarmMedicalAlert
{
    [HarmonyPatch(typeof(Alert_LifeThreateningHediff), "get_SickPawns")]
    static class Patch_Alert_LifeThreateningHediff
    {
        static void Postfix(ref List<Pawn> __result)
        {
            List<Pawn> tmpList = __result.ListFullCopy();
            for (int i = 0; i < __result.Count; i++)
            {
                if (__result[i].guest is Pawn_GuestTracker guestTracker && guestTracker.interactionMode == PrisonerInteractionModeDefOf.HemogenFarm)
                {
                    tmpList.Remove(__result[i]);  
                }
            }
            __result = tmpList;
        }
    }
}
