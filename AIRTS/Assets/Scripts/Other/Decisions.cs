// Script by: Tristan Bampton Up690813

using UnityEngine;
using System.Collections;
using System;

namespace Decisions
{
    public interface IDecision
    {
        object Test();
    }

    public class ObjectDecision : IDecision
    {
        public delegate object ObjectParameter();

        public ObjectParameter Decision;

        public ObjectDecision() { }

        public ObjectDecision(ObjectParameter decision)
        {
            Decision = decision;
        }

        public object Test()
        {
            return Decision();
        }
    }

}