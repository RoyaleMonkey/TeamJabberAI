using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace GoldLeader
{
    public class Task_Shoot : Action
    {
        public SharedGameObject behaviorGameObject;
        public float energyToSave = 0;

        GoldLeaderWeaponSystem weaponSystem;


        public override void OnAwake()
        {
            weaponSystem = GetDefaultGameObject(behaviorGameObject.Value).GetComponent<GoldLeaderWeaponSystem>();
        }

        public override TaskStatus OnUpdate()
        {
            UnityEngine.Debug.Log("Je shoot");
            weaponSystem.FireShoot(energyToSave);
            return TaskStatus.Success;
        }
    }
}
