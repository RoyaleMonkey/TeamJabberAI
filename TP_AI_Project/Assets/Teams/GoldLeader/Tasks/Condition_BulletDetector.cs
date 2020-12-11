using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace GoldLeader
{

    [TaskCategory("Unity/SharedVariable")]
    [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
    public class Condition_BulletDetector : Conditional
    {

        public SharedGameObject behaviorGameObject;
        public float detectionRange;
        public float threatAngle;

        GoldLeaderDefenseSystem defenseSystem;

        public override void OnAwake()
        {
            defenseSystem = GetDefaultGameObject(behaviorGameObject.Value).GetComponent<GoldLeaderDefenseSystem>();
        }

        public override TaskStatus OnUpdate()
        {
            return defenseSystem.DetectIncomingBullet(detectionRange, threatAngle) ? TaskStatus.Success : TaskStatus.Failure;
            /*switch (comparer)
            {
                case Comparer.Equal:
                    return variable.Value.Equals(compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
                case Comparer.Superior:
                    return (variable.Value > compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
                case Comparer.Inferior:
                    return (variable.Value < compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
            }*/
            //return TaskStatus.Success;
            //return variable.Value.Equals(compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {

        }
    }
}
