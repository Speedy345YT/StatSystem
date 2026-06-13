using System;
using System.Collections.Generic;
using UnityEngine;
namespace StatSystem
{
    [Serializable]
    public class VectorStat : Stat<Vector3>
    {
        public VectorStat(Vector3 baseValue)
        {
            BaseValue = baseValue;
            isDirty = true;
        }

        public override StatModifier<Vector3> AddModifier(StatModifier<Vector3> mod)
        {
            var existing = modifiers.Find(m => m == mod);

            if (existing == null)
            {
                modifiers.Add(mod);
            }

            isDirty = true;
            return mod;
        }
        public override StatModifier<Vector3> AddModifier(Vector3 value, StatModifierType type)
        {
            var mod = new StatModifier<Vector3>(value, type);
            var existing = modifiers.Find(m => m == mod);

            if (existing == null)
            {
                modifiers.Add(mod);
            }

            isDirty = true;
            return mod;
        }
        public override void RemoveModifier(StatModifier<Vector3> mod)
        {
            var existing = modifiers.Find(m => m == mod);

            if (existing != null)
            {
                modifiers.Remove(mod);

                isDirty = true;

            }
        }
        public override void ClearModifier(StatModifier<Vector3> mod)
        {
            var existing = modifiers.Find(m => m == mod);

            if (existing != null)
            {
                modifiers.Remove(existing);

                isDirty = true;

            }
        }
        public override void UpdateModifier(StatModifier<Vector3> mod, Vector3 value)
        {
            var existing = modifiers.Find(m => m == mod);

            if (existing != null)
            {
                existing.value = value;

                isDirty = true;
            }
        }
        public override Vector3 Value
        {
            get
            {
                if (!isDirty)
                    return CachedValue;

                Vector3 flatSum = Vector3.zero;
                Vector3 additiveMultSum = Vector3.zero;
                Vector3 multiplicativeMultProduct = Vector3.one;
                Vector3 setValue = Vector3.zero;
                bool hasSet = false;

                if (modifiers.Count != 0)
                {
                    foreach (var mod in modifiers)
                    {
                        switch (mod.type)
                        {
                            case StatModifierType.Flat:
                                flatSum += mod.value;
                                break;

                            case StatModifierType.AdditiveMultiplier:
                                additiveMultSum += mod.value;
                                break;

                            case StatModifierType.MultiplicativeMultiplier:
                                multiplicativeMultProduct = Vector3.Scale(multiplicativeMultProduct, mod.value);
                                break;
                            case StatModifierType.Set:
                                if (!hasSet)
                                {
                                    setValue = BaseValue;
                                    hasSet = true;
                                }

                                if (mod.value.x != BaseValue.x) setValue.x = mod.value.x;
                                if (mod.value.y != BaseValue.y) setValue.y = mod.value.y;
                                if (mod.value.z != BaseValue.z) setValue.z = mod.value.z;
                                break;
                        }
                    }
                }

                Vector3 baseToUse = hasSet ? setValue : BaseValue;

                CachedValue = Vector3.Scale(baseToUse + flatSum, (Vector3.one + additiveMultSum));

                CachedValue = Vector3.Scale(CachedValue, multiplicativeMultProduct);
                isDirty = false;

                return CachedValue;
            }
        }
        public float x => Value.x;
        public float y => Value.y;
        public float z => Value.z;
        public float Magnitude => Value.magnitude;
        public Vector3 Normalized => Value.normalized;
        public Vector3 ChangePercent => new Vector3(Value.x / BaseValue.x, Value.y / BaseValue.y, Value.z / BaseValue.z);
    }
}