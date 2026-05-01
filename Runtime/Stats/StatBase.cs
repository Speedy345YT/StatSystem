using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace StatSystem
{
    public interface Stat
    {
        void RemoveModifier(StatModifierBase mod);
    }
    #if ODIN_INSPECTOR
    [InlineProperty(LabelWidth = 100)]
    #endif
    public abstract class Stat<T> : Stat
    {
        public abstract StatModifier<T> AddModifier(StatModifier<T> mod);
        public abstract StatModifier<T> AddModifier(T value, StatModifierType type);
        public abstract void RemoveModifier(StatModifier<T> mod);
        public abstract void ClearModifier(StatModifier<T> mod);
        public abstract void UpdateModifier(StatModifier<T> mod, T value);
        public void RemoveModifier(StatModifierBase mod)
        {
            RemoveModifier(mod as StatModifier<float>);
        }
        public virtual T Value { get; }
        public T BaseValue;
        public T CachedValue;
        protected bool isDirty;
        public List<StatModifier<T>> modifiers = new();
    }
    public abstract class StatModifierBase
    {
        public StatModifierType type;
    }
    public class StatModifier<T> : StatModifierBase
    {
        public T value;
        public StatModifier(T value, StatModifierType type)
        {
            this.value = value;
            this.type = type;
        }
    }
    public enum StatModifierType
    {
        Flat, //+5
        AdditiveMultiplier, //+20%
        MultiplicativeMultiplier, //*2
        Set //=2
    }
}
