using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
    [Serializable]
    public class FloatStat : Stat<float>
    {
        public FloatStat(float baseValue)
        {
            BaseValue = baseValue;
            isDirty = true;
        }

        public override StatModifier<float> AddModifier(StatModifier<float> mod)
        {
            var existing = modifiers.Find(m => m == mod);

            if (existing == null)
            {
                modifiers.Add(mod);
            }

            isDirty = true;
            return mod;
        }
        public override StatModifier<float> AddModifier(float value, StatModifierType type)
        {
            var mod = new StatModifier<float>(value, type);
            var existing = modifiers.Find(m => m == mod);

            if (existing == null)
            {
                modifiers.Add(mod);
            }

            isDirty = true;
            return mod;
        }
        public override void RemoveModifier(StatModifier<float> mod)
        {
            var existing = modifiers.Find(m => m == mod);

            if (existing != null)
            {
                modifiers.Remove(mod);

                isDirty = true;
            }
        }
        public override void ClearModifier(StatModifier<float> mod)
        {
            var existing = modifiers.Find(m => m == mod);

            if (existing != null)
            {
                modifiers.Remove(existing);

                isDirty = true;

            }
        }
        public override void UpdateModifier(StatModifier<float> mod, float value)
        {
            var existing = modifiers.Find(m => m == mod);

            if (existing != null)
            {
                existing.value = value;

                isDirty = true;
            }
        }

        public override float Value
        {
            get
            {
                if (!isDirty)
                    return CachedValue;

                float flatSum = 0;
                float additiveMultSum = 0;
                float multiplicativeMultProduct = 1f;
                float setValue = 0;
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
                                multiplicativeMultProduct *= mod.value;
                                break;
                            case StatModifierType.Set:
                                setValue = mod.value;
                                hasSet = true;
                                break;
                        }
                    }
                }
                float baseToUse = hasSet ? setValue : BaseValue;

                CachedValue = (baseToUse + flatSum) * (1f + additiveMultSum) * multiplicativeMultProduct;
                isDirty = false;

                return CachedValue;
            }
        }
        public int FloorValue => Mathf.FloorToInt(Value);
        public int RoundValue => Mathf.RoundToInt(Value);
        public int CeilValue => Mathf.CeilToInt(Value);
        public float ChangeValue => Value / BaseValue;
        public float ChangePercent => Value / BaseValue * 100;
    }
}