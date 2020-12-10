using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Eagle
{
	public class CutBuster : Action
	{
		BehaviorTree _behaviorTree;

		public override void OnStart()
		{
			_behaviorTree = GetComponent<BehaviorTree>();

			_behaviorTree.SetVariableValue("thrust", 0.0f);
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}
	}
}