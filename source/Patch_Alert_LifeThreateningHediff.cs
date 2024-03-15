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
            if (__result.Count <= 0)
                return;

            List<Pawn> tmpList = null;
            for (int i = 0; i < __result.Count; i++)
            {
                Pawn pawn = __result[i];
#if v1_4
                if (pawn.guest is Pawn_GuestTracker guestTracker && (guestTracker.interactionMode == PrisonerInteractionModeDefOf.HemogenFarm || guestTracker.interactionMode == PrisonerInteractionModeDefOf.Bloodfeed))
#endif
#if v1_5
                if (pawn.guest is Pawn_GuestTracker guestTracker && (guestTracker.IsInteractionEnabled(PrisonerInteractionModeDefOf.HemogenFarm) || guestTracker.IsInteractionEnabled(PrisonerInteractionModeDefOf.Bloodfeed)))
#endif
                {
                    bool hasOtherLifeThreateningHediff = false;
                    for (int j = 0; j < pawn.health.hediffSet.hediffs.Count; j++)
                    {
                        Hediff hediff = pawn.health.hediffSet.hediffs[j];
                        if (hediff.CurStage == null || !hediff.CurStage.lifeThreatening || hediff.FullyImmune())
                            continue;

                        if (hediff.def == HediffDefOf.BloodLoss)
                            continue;

                        if (hediff.def == HediffDefOf.GeneticDrugNeed && hediff.Severity < 11.0) // 0.2 per day
                            continue;

                        hasOtherLifeThreateningHediff = true;
                        break;
                    }
                    if (!hasOtherLifeThreateningHediff)
                    {
                        if (tmpList == null)
                            tmpList = __result.ListFullCopy();
                        tmpList.Remove(__result[i]);  
                    }
                }
            }
            
            if (tmpList != null)
                __result = tmpList;
        }
    }
}
