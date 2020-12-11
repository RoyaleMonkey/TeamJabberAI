using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace GoldLeader
{
    public class Task_MoveAndAttack : Action
    {
        public SharedGameObject behaviorGameObject;

        GoldLeaderMovementSystem movementSystem;
        GoldLeaderWeaponSystem weaponSystem;

        public override void OnAwake()
        {
            movementSystem = GetDefaultGameObject(behaviorGameObject.Value).GetComponent<GoldLeaderMovementSystem>();
            weaponSystem = GetDefaultGameObject(behaviorGameObject.Value).GetComponent<GoldLeaderWeaponSystem>();
        }

        public override TaskStatus OnUpdate()
        {
            if (movementSystem.MoveToEnemy(weaponSystem.CanShootIntersect, weaponSystem.ShootIntersection) == true)
                return TaskStatus.Success;
            return TaskStatus.Running;

        }
    }
}
