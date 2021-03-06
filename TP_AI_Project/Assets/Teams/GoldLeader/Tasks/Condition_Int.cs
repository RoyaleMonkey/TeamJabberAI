﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace GoldLeader
{
    [TaskCategory("Unity/SharedVariable")]
    [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
    public class Condition_Int : Conditional
    {
        //[Tooltip("The first variable to compare")]
        public SharedInt variable;
        //[Tooltip("The variable to compare to")]
        public Comparer comparer;
        public SharedInt compareTo;

        public override TaskStatus OnUpdate()
        {
            switch(comparer)
            {
                case Comparer.Equal:
                    return variable.Value.Equals(compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
                case Comparer.Superior:
                    return (variable.Value > compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
                case Comparer.Inferior:
                    return (variable.Value < compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
            }
            return TaskStatus.Success;
            //return variable.Value.Equals(compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            variable = 0;
            compareTo = 0;
        }
    }
}