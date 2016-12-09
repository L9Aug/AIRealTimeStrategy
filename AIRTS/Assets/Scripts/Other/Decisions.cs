// Script by: Tristan Bampton Up690813

using UnityEngine;
using System.Collections;
using System;

namespace Decisions
{
    public abstract class ADecision
    {
        public abstract object Test();
    }

    public class ObjectDecision : ADecision
    {
        public delegate object ObjectParameter();

        public ObjectParameter Decision;

        public ObjectDecision() { }

        public ObjectDecision(ObjectParameter decision)
        {
            Decision = decision;
        }

        public override object Test()
        {
            return Decision();
        }
    }

}