using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate float FloatParameter();

public delegate bool BoolParameter();

public class Conditions : MonoBehaviour {
}

public interface ICondition
{
    bool Test();
}

public class FloatCondition : ICondition
{
    public float minValue;
    public float maxValue;

    public FloatParameter TestValue;

    public bool Test()
    {
        return minValue <= TestValue() && TestValue() <= maxValue;
    }
}

public class AndCondition : ICondition
{
    public ICondition A;
    public ICondition B;

    public bool Test()
    {
        return A.Test() && B.Test();
    }
}

public class OrCondition : ICondition
{
    public ICondition A;
    public ICondition B;

    public bool Test()
    {
        return A.Test() || B.Test();
    }
}

public class NotCondition : ICondition
{
    public ICondition condition;
    public bool Test()
    {
        return !condition.Test();
    }
}

public class ALessThanB : ICondition
{
    public float A = 0;
    public float B = 0;

    public bool Test()
    {
        return A < B;
    }
}

public class AGreaterThanB : ICondition
{
    public float A;
    public float B;

    public bool Test()
    {
        return A > B;
    }
}

public class AEqualsB : ICondition
{
    public float A;
    public float B;

    public bool Test()
    {
        return A == B;
    }
}

public class NullGameobject : ICondition
{
    public GameObject obj;

    public bool Test()
    {
        if (obj == null)
            return true;
        else
            return false;
    }
}

public class SingleResetBool : ICondition
{
    public bool A;

    public bool Test()
    {
        bool result = A;

        if (A)
            A = false;
        return result;
    }
}

public class SingleBool : ICondition
{
    public bool A;

    public bool Test()
    {
        return A;
    }
}

public class ListHasDataCond<T> : ICondition
{
    public List<T> A;
    public bool Test()
    {
        if (A.Count > 0)
            return true;
        else
            return false;
    }
}

public class ListNotNullCond<T> : ICondition
{
    public List<T> list;

    public bool Test()
    {
        bool trigger = true;
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i] == null)
                {
                    trigger = false;
                    break;
                }
            }
            return trigger;
        }
        else
        {
            return false;
        }
        return trigger;
    }

    public class ListAllTrueCond: ICondition
    {
        public List<bool> list;

        public bool Test()
        {
            if (list.Contains(false))
                return false;
            else
                return true;
        }
    }
}