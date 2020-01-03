﻿using System;
using System.Linq;
using BattleTech;
using Harmony;

namespace MechEngineer.Features.HardpointFix.sorting.Patches
{
    [HarmonyPatch(typeof(HardpointDataDef), nameof(HardpointDataDef.FromJSON))]
    public static class HardpointDataDef_FromJSON_Patch
    {
        public static void Postfix(HardpointDataDef __instance)
        {
            try
            {
                HardpointDataDef_FromJSON(__instance);
            }
            catch (Exception e)
            {
                Control.mod.Logger.LogError(e);
            }
        }

        internal static void HardpointDataDef_FromJSON(HardpointDataDef def)
        {
            if (!HardpointFixFeature.Shared.Settings.AutoFixHardpointDataDefs)
            {
                return;
            }

            // chrPrfWeap_battlemaster_leftarm_ac20_bh1 -> chrPrfWeap_battlemaster_leftarm_ac20_1
            string Unique(string prefab)
            {
                return prefab.Substring(0, prefab.Length - 3) + GroupNumber(prefab);
            }

            // chrPrfWeap_battlemaster_leftarm_ac20_bh1 -> 1
            string GroupNumber(string prefab)
            {
                return prefab.Substring(prefab.Length - 1, 1);
            }

            // normalize data, remove all duplicates in each location
            for (var i = 0; i < def.HardpointData.Length; i++)
            {
                def.HardpointData[i].weapons = def.HardpointData[i].weapons
                    .SelectMany(x => x)
                    .OrderBy(x => x)
                    .GroupBy(x => Unique(x))
                    .Select(x => x.First())
                    .GroupBy(x => GroupNumber(x))
                    .Select(x => x.ToArray())
                    .ToArray();
            }
        }
    }
}