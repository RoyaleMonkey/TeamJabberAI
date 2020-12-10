using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using DoNotModify;

namespace Eagle {

	public class AI_Eagle : BaseSpaceShipController
	{
		private Blackboard _blackboard;

		private BehaviorTree _behaviorTree;

		private float elapsedTimeBomb = 0;
		private float elapsedTimeShockwave = 0;

		public override void Initialize(SpaceShip spaceship, GameData data)
		{
			_behaviorTree = GetComponent<BehaviorTree>();

			_blackboard = GetComponent<Blackboard>();
			if (!_blackboard)
            {
				_blackboard = gameObject.AddComponent(typeof(Blackboard)) as Blackboard;
			}

			_blackboard.Initialize(spaceship, data);
		}

		public override InputData UpdateInput(SpaceShip spaceship, GameData data)
		{
			float timerBomb = (_behaviorTree.GetVariable("TimerBomb") as SharedFloat).Value;
			float timerShockwave = (_behaviorTree.GetVariable("TimerShockwave") as SharedFloat).Value;			

			float thrust = (_behaviorTree.GetVariable("thrust") as SharedFloat).Value;
			float targetOrient = (_behaviorTree.GetVariable("targetOrient") as SharedFloat).Value;

			bool IsDropping = (_behaviorTree.GetVariable("IsDroppingMine") as SharedBool).Value;
			bool IsFiringShockwave = (_behaviorTree.GetVariable("IsFiringShockwave") as SharedBool).Value;

			if (timerBomb != 0)
			{
				elapsedTimeBomb += Time.deltaTime;
				if (elapsedTimeBomb >= timerBomb)
				{
					_behaviorTree.SetVariableValue("IsDroppingMine", true);
					IsDropping = (_behaviorTree.GetVariable("IsDroppingMine") as SharedBool).Value;
					//_behaviorTree.SetVariableValue("TimerBomb", 0);
					elapsedTimeBomb = 0;
				}
			}

			if (timerShockwave != 0)
			{
				elapsedTimeShockwave += Time.deltaTime;
				if (elapsedTimeShockwave >= timerShockwave)
				{
					_behaviorTree.SetVariableValue("IsFiringShockwave", true);
					IsDropping = (_behaviorTree.GetVariable("IsFiringShockwave") as SharedBool).Value;
					_behaviorTree.SetVariableValue("TimerShockwave", 0);
					elapsedTimeShockwave = 0;
				}
			}

			_behaviorTree.SetVariableValue("IsDroppingMine", false);
			_behaviorTree.SetVariableValue("IsFiringShockwave", false);

			_blackboard.UpdateData(data);

			bool triggerShoot = (_behaviorTree.GetVariable("TriggerShoot") as SharedBool).Value;
			if (_blackboard.TriggerShoot)
				triggerShoot = true;
			_behaviorTree.SetVariableValue("TriggerShoot", false);

			return new InputData(thrust, targetOrient, triggerShoot, IsDropping, IsFiringShockwave);
		}
	}
}
