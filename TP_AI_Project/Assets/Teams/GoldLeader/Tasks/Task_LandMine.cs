using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace GoldLeader
{
    public class Task_LandMine : Action
    {
        public SharedGameObject behaviorGameObject;
        public float energyToSave = 0;

        GoldLeaderMovementSystem movementSystem;
        GoldLeaderWeaponSystem weaponSystem;


        public override void OnAwake()
        {
            weaponSystem = GetDefaultGameObject(behaviorGameObject.Value).GetComponent<GoldLeaderWeaponSystem>();
            movementSystem = GetDefaultGameObject(behaviorGameObject.Value).GetComponent<GoldLeaderMovementSystem>();
        }

        public override TaskStatus OnUpdate()
        {
            if(movementSystem.currentWayPoint.Position == movementSystem.currentWayPoint.WayPoint.transform.position)
                weaponSystem.LandMine(energyToSave, 0.01f);
            return TaskStatus.Success;
        }
    }
}
