using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Eagle
{
	public class ActivateBuster : Action
	{
		[Range(0f, 1f)]
		public SharedFloat speed = 1f;

		BehaviorTree _behaviorTree;
		public override void OnStart()
		{
			_behaviorTree = GetComponent<BehaviorTree>();
		}

		public override TaskStatus OnUpdate()
		{
			_behaviorTree.SetVariableValue("thrust", speed);
			return TaskStatus.Success;
		}
	}
}