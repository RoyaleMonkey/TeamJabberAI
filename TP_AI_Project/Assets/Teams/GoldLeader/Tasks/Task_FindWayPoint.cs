using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace GoldLeader
{
    public class Task_FindWayPoint : Action
    {
        public SharedGameObject behaviorGameObject;

        GoldLeaderMovementSystem movementSystem;

        public override void OnAwake()
        {
            movementSystem = GetDefaultGameObject(behaviorGameObject.Value).GetComponent<GoldLeaderMovementSystem>();
        }

        public override TaskStatus OnUpdate()
        {
            movementSystem.FindWaypoint();
            return TaskStatus.Success;
        }
    }
}
