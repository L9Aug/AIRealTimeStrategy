using UnityEngine;
using System.Collections;

namespace Condition
{

    public interface ICondition
    {
        bool Test();
    }

    /// <summary>
    /// Test Value between Min and Max values
    /// </summary>
    public class FloatCondition : ICondition
    {
        public float MinValue;
        public float MaxValue;

        public delegate float FloatParam();
        public FloatParam TestValue;

        bool ICondition.Test()
        {
            return (MinValue <= TestValue()) && (TestValue() <= MaxValue);
        }
    }

    /// <summary>
    /// A less than or equal to B
    /// </summary>
    public class LessThanFloatCondition : ICondition
    {
        public delegate float FloatParam();
        public FloatParam A;
        public FloatParam B;

        bool ICondition.Test()
        {
            return A() <= B();
        }
    }

    /// <summary>
    /// A greater than or equal to B
    /// </summary>
    public class GreaterThanFloatCondition : ICondition
    {
        public delegate float FloatParam();
        public FloatParam A;
        public FloatParam B;

        bool ICondition.Test()
        {
            return A() >= B();
        }
    }

    public class BoolCondition : ICondition
    {
        public delegate bool BoolParam();
        public BoolParam Condition;

        bool ICondition.Test()
        {
            return Condition();
        }
    }

    public class AndCondition : ICondition
    {
        public ICondition ConditionA;
        public ICondition ConditionB;

        bool ICondition.Test()
        {
            return ConditionA.Test() && ConditionB.Test();
        }
    }

    public class OrCondition : ICondition
    {
        public ICondition ConditionA;
        public ICondition ConditionB;

        bool ICondition.Test()
        {
            return ConditionA.Test() || ConditionB.Test();
        }
    }

    public class NotCondition : ICondition
    {
        public ICondition Condition;

        bool ICondition.Test()
        {
            return !Condition.Test();
        }
    }

    /// <summary>
    /// True if null
    /// </summary>
    public class NullCondition : ICondition
    {
        public delegate object ObjectParam();
        public ObjectParam Condition;

        bool ICondition.Test()
        {
            return Condition() == null;
        }
    }
}
