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

            List<Pawn> tmpList = __result.ListFullCopy();
            for (int i = 0; i < __result.Count; i++)
            {
                Pawn pawn = __result[i];
                if (pawn.guest is Pawn_GuestTracker guestTracker && guestTracker.interactionMode == PrisonerInteractionModeDefOf.HemogenFarm)
                {
                    bool hasLifeThreateningHediffNotBloodLoss = false;
                    for (int j = 0; j < pawn.health.hediffSet.hediffs.Count; j++)
                    {
                        Hediff hediff = pawn.health.hediffSet.hediffs[j];
                        if (hediff.CurStage != null && hediff.CurStage.lifeThreatening && !hediff.FullyImmune() && hediff.def != HediffDefOf.BloodLoss)
                        {
                            hasLifeThreateningHediffNotBloodLoss = true;
                        }
                    }
                    if (!hasLifeThreateningHediffNotBloodLoss)
                        tmpList.Remove(__result[i]);  
                }
            }
            __result = tmpList;
        }
    }
}
