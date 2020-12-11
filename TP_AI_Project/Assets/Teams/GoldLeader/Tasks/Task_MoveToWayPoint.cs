using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace GoldLeader
{
    public class Task_MoveToWayPoint : Action
    {
        public SharedGameObject behaviorGameObject;

        GoldLeaderMovementSystem movementSystem;

        public override void OnAwake()
        {
            movementSystem = GetDefaultGameObject(behaviorGameObject.Value).GetComponent<GoldLeaderMovementSystem>();
        }

        public override TaskStatus OnUpdate()
        {
            if (movementSystem.MoveToPoint() == true)
                return TaskStatus.Success;
            return TaskStatus.Running;
        }
    }
}
