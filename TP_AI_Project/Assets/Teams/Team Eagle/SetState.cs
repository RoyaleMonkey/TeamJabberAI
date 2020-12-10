using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Eagle
{
	public class SetState : Action
	{
		BehaviorTree _behaviorTree;

		public SharedString state;
		public override void OnStart()
		{
			_behaviorTree = GetComponent<BehaviorTree>();
		}

		public override TaskStatus OnUpdate()
		{
			_behaviorTree.SetVariableValue("State", state);

			return TaskStatus.Success;
		}
	}
}