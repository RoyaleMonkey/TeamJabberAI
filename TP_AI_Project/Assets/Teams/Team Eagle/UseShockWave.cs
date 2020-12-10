using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Eagle
{
	[TaskCategory("AI_Eagle")]
	public class UseShockWave : Action
	{
		SharedFloat IsMiningWeight = 0f;

		public SharedFloat highWeightValueE = 0.2f;
		public SharedFloat midWeightValueE = 0.1f;
		public SharedFloat lowWeightValueE = -1f;

		public SharedFloat highWeightValueD = 0.6f;
		public SharedFloat midWeightValueD = 0.4f;
		public SharedFloat lowWeightValueD = -1f;

		public SharedFloat highWeightValueT = 0.2f;
		public SharedFloat midWeightValueT = 0.1f;
		public SharedFloat lowWeightValueT = -0f;


		public SharedFloat highEnergy = 0.8f;
		public SharedFloat lowEnergy = 0.4f;

		public SharedFloat highDistance = 6;
		public SharedFloat lowDistance = 3;

		public SharedFloat highTime = 40f;
		public SharedFloat lowTime = 20f;

		BehaviorTree _behaviorTree;
		public override void OnStart()
		{
			_behaviorTree = GetComponent<BehaviorTree>();	
		}

		private float EnergyWeight(GameData data, int _owner)
		{
			float weightValue;

			if (data.SpaceShips[_owner].Energy >= highEnergy.Value)
			{
				weightValue = highWeightValueE.Value;

			}
			else if (data.SpaceShips[_owner].Energy >= lowEnergy.Value)
			{
				weightValue = midWeightValueE.Value;
			}
			else
			{
				weightValue = lowWeightValueE.Value;
			}

			return weightValue;
		}

		private float DistanceWeight(GameData data, int _owner)
		{
			float weightValue = 0;

			for (int i = 0; i < data.SpaceShips.Count; i++)
			{
				if (data.SpaceShips[i] != data.SpaceShips[_owner])
				{
					float tmpDist = Vector2.Distance(data.SpaceShips[i].Position, data.SpaceShips[_owner].Position);

					if (tmpDist <= data.SpaceShips[_owner].transform.lossyScale.x * lowDistance.Value)
					{
						weightValue = highWeightValueD.Value;
					}
					else if (tmpDist <= data.SpaceShips[_owner].transform.lossyScale.x * highDistance.Value)
					{
						weightValue = midWeightValueD.Value;
					}
					else
					{
						weightValue = lowWeightValueD.Value;
					}
				}
			}

			return weightValue;
		}

		private float TimeWeight(GameData data, int _owner)
		{
			float weightValue;

			if (data.timeLeft <= lowTime.Value)
			{
				weightValue = lowWeightValueT.Value;
			}
			else if (data.timeLeft <= highTime.Value)
			{
				weightValue = midWeightValueT.Value;
			}
			else
			{
				weightValue = highWeightValueT.Value;
			}

			return weightValue;
		}

		public override TaskStatus OnUpdate()
		{
			GameData data = (_behaviorTree.GetVariable("GameData") as SharedGameData).Value;
			int _owner = (_behaviorTree.GetVariable("Owner") as SharedInt).Value;

			bool IsFiring = false;

			IsMiningWeight.Value = EnergyWeight(data, _owner) + DistanceWeight(data, _owner) + TimeWeight(data, _owner);

			if (IsMiningWeight.Value >= 0.5)
			{
				_behaviorTree.SetVariableValue("TimerShockwave", 0f);
				IsFiring = true;
			}
			else if (IsMiningWeight.Value >= 0)
			{
				if ((_behaviorTree.GetVariable("TimerShockwave") as SharedFloat).Value != 0)
				{
					_behaviorTree.SetVariableValue("TimerShockwave", 1f);
					IsFiring = false;
				}
				else
				{
					_behaviorTree.SetVariableValue("TimerShockwave", 0f);
					IsFiring = true;
				}
			}
			else
			{
				IsFiring = false;
			}

			_behaviorTree.SetVariableValue("IsFiringShockwave", IsFiring);

			IsMiningWeight = 0;

			return TaskStatus.Success;
		}
	}
}