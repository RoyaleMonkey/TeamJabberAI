using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Eagle
{
	[TaskCategory("AI_Eagle")]
	public class EnemyinRange : Action
	{
		[Range(1f, 3f)]
		public SharedFloat DetecRadius = 1f;

		BehaviorTree _behaviorTree;
		public override void OnStart()
		{
			_behaviorTree = GetComponent<BehaviorTree>();
		}

		public override TaskStatus OnUpdate()
		{

			GameData data = (_behaviorTree.GetVariable("GameData") as SharedGameData).Value;
			int _owner = (_behaviorTree.GetVariable("Owner") as SharedInt).Value;

			for (int i = 0; i < data.SpaceShips.Count; i++)
			{
                if (data.SpaceShips[i] != data.SpaceShips[_owner])
                {
					float tmpDist = Vector2.Distance(data.SpaceShips[i].Position, data.SpaceShips[_owner].Position);

					if (tmpDist > DetecRadius.Value)
					{
						return TaskStatus.Success;
					}
				}
			}

			return TaskStatus.Success;
		}
	}
}