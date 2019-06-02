﻿using System.Linq;
using BattleTech;
using CustomComponents;

namespace MechEngineer.Features.ArmorStructureChanges
{
    internal class ArmorStructureChangesFeature : Feature
    {
        internal static ArmorStructureChangesFeature Shared = new ArmorStructureChangesFeature();

        internal override bool Enabled => settings?.Enabled ?? false;

        internal static Settings settings => Control.settings.ArmorStructureChanges;

        public class Settings
        {
            public bool Enabled = true;
        }

        internal static float GetArmorFactorForMech(Mech mech)
        {
            const string key = "ArmorMultiplier";
            var statistic = mech.StatCollection.GetStatistic(key)
                            ?? mech.StatCollection.AddStatistic<float>(key, GetArmorFactorForMechDef(mech.MechDef));
            return statistic.Value<float>();
        }

        internal static float GetStructureFactorForMech(Mech mech)
        {
            const string key = "StructureMultiplier";
            var statistic = mech.StatCollection.GetStatistic(key)
                            ?? mech.StatCollection.AddStatistic(key, GetStructureFactorForMechDef(mech.MechDef));
            return statistic.Value<float>();
        }

        private static float GetArmorFactorForMechDef(MechDef mechDef)
        {
            return mechDef.Inventory
                .Select(r => r.GetComponent<ArmorStructureChanges>())
                .Where(c => c != null)
                .Select(c => c.ArmorFactor)
                .Aggregate(1.0f, (acc, val) => acc * val);
        }

        private static float GetStructureFactorForMechDef(MechDef mechDef)
        {
            return mechDef.Inventory
                .Select(r => r.GetComponent<ArmorStructureChanges>())
                .Where(c => c != null)
                .Select(c => c.StructureFactor)
                .Aggregate(1.0f, (acc, val) => acc * val);
        }
    }
}
