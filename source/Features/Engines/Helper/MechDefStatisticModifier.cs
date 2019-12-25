﻿using System.Linq;
using BattleTech;

namespace MechEngineer.Features.Engines.Helper
{
    internal static class MechDefStatisticModifier
    {
        internal static T ModifyStatistic<T>(StatisticAdapter<T> stat, MechDef mechDef)
        {
            foreach (var componentDef in mechDef.Inventory.Where(x => x.IsFunctionalORInstalling()).Select(x => x.Def))
            {
                if (componentDef.statusEffects == null)
                {
                    continue;
                }

                foreach (var effectData in componentDef.statusEffects)
                {
                    if (effectData.targetingData.effectTriggerType != EffectTriggerType.Passive
                        || effectData.targetingData.effectTargetType != EffectTargetType.Creator)
                    {
                        continue;
                    }
                    if (stat.Key != effectData.statisticData.statName)
                    {
                        continue;
                    }
                    stat.Modify(effectData);
                }
            }
            return stat.Get();
        }
    }
}