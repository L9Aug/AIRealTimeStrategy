using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Condition
{

    public delegate float FloatParameter();

    public delegate bool BoolParameter();

    public interface ICondition
    {
        bool Test();
    }

    public class FloatCondition : ICondition
    {
        public float MinValue;
        public float MaxValue;
        public FloatParameter TestValue;

        public FloatCondition() { }

        public FloatCondition(float minValue, float maxValue, FloatParameter testValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            TestValue = testValue;
        }

        public bool Test()
        {
            return MinValue <= TestValue() && TestValue() <= MaxValue;
        }
    }

    public class AndCondition : ICondition
    {
        public ICondition A;
        public ICondition B;

        public AndCondition() { }

        public AndCondition(ICondition a, ICondition b)
        {
            A = a;
            B = b;
        }

        public bool Test()
        {
            return A.Test() && B.Test();
        }
    }

    public class OrCondition : ICondition
    {
        public ICondition A;
        public ICondition B;

        public OrCondition() { }

        public OrCondition(ICondition a, ICondition b)
        {
            A = a;
            B = b;
        }

        public bool Test()
        {
            return A.Test() || B.Test();
        }
    }

    public class NotCondition : ICondition
    {
        public ICondition Condition;

        public NotCondition() { }

        public NotCondition(ICondition condition)
        {
            Condition = condition;
        }

        public bool Test()
        {
            return !Condition.Test();
        }
    }

    public class ALessThanB : ICondition
    {
        public FloatParameter A;
        public FloatParameter B;

        public ALessThanB() { }

        public ALessThanB(FloatParameter a, FloatParameter b)
        {
            A = a;
            B = b;
        }

        public bool Test()
        {
            return A() < B();
        }
    }

    public class AGreaterThanB : ICondition
    {
        public FloatParameter A;
        public FloatParameter B;

        public AGreaterThanB() { }

        public AGreaterThanB(FloatParameter a, FloatParameter b)
        {
            A = a;
            B = b;
        }

        public bool Test()
        {
            return A() > B();
        }
    }

    public class AEqualsB : ICondition
    {
        public FloatParameter A;
        public FloatParameter B;

        public AEqualsB() { }

        public AEqualsB(FloatParameter a, FloatParameter b)
        {
            A = a;
            B = b;
        }

        public bool Test()
        {
            return A() == B();
        }
    }

    public class NullObject<T> : ICondition
    {
        public T Obj;

        public NullObject() { }

        public NullObject(T obj)
        {
            Obj = obj;
        }
       
        public bool Test()
        {
            return ((Obj == null) ? true : false);
        }
    }

    public class BoolCondition : ICondition
    {
        public BoolParameter Condition;

        public BoolCondition() { }

        public BoolCondition(BoolParameter condition)
        {
            Condition = condition;
        }

        bool ICondition.Test()
        {
            return Condition();
        }
    }

    public class ListHasDataCond<T> : ICondition
    {
        public List<T> TestList;

        public ListHasDataCond() { }

        public ListHasDataCond(ref List<T> list)
        {
            TestList = list;
        }

        public bool Test()
        {
            return (TestList.Count > 0) ? true : false;
        }
    }

    public class HexPositionEqual : ICondition
    {
        public delegate HexTransform GetHexTransform();

        public GetHexTransform PositionA;
        public GetHexTransform PositionB;

        public HexPositionEqual() { }

        public HexPositionEqual(GetHexTransform positionA, GetHexTransform positionB)
        {
            PositionA = positionA;
            PositionB = positionB;
        }

        public bool Test()
        {
            return (PositionA().Position == PositionB().Position) ? true : false;
        }
    }

}